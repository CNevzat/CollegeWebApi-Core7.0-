using CollegeApp.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            _logger.LogInformation("Get Students method started");
            var students = CollegeRepository.Students.Select(n => new StudentDTO()
            {
                ID = n.ID,
                StudentName = n.StudentName,
                Address = n.Address,
                Email = n.Email,           
            });
            // OK - 200 - Success
            return Ok(CollegeRepository.Students);
        }





        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)] //undocumented olmaması için
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            // BadRequest - 400 - Client Error
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }

            var student = CollegeRepository.Students.Where(n => n.ID == id).FirstOrDefault();
            // NotFound - 404 - Clint Error
            if (student == null)
            {
                _logger.LogError("Student not found with given Id");
                return NotFound($"The student with id {id} not found");
            }

            var studentDTO = new StudentDTO()
            {
                ID = student.ID,
                StudentName = student.StudentName,
                Address = student.Address,
                Email = student.Email
            };
            // OK - 200 - Success
            return Ok(student);
        }



        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/student/create
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {

            if (model == null)
            {
                return BadRequest();
            }
            //if (model.AdmissionDate < DateTime.Now)
            //{
            //    // 1. Directly adding error message to modelstate
            //    // 2. Using custom atribute
            //    //ModelState.AddModelError("AdmissionDate Error", "Admission date must be greater than or equal to todays date");
            //    //return BadRequest(ModelState);
            //}
            int newId = CollegeRepository.Students.LastOrDefault().ID + 1;
            Student student = new Student()
            {
                ID = newId,
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email
            };
            CollegeRepository.Students.Add(student);
            model.ID = student.ID;
            //Status - 201
            //https://localhost:7008/api/Student/3 yeni öğrencinin url'si böyle olucak
            //New student details
            return CreatedAtRoute("GetStudentById" , new {id= model.ID}, model); // Yeni kayıt eklendikten sonra GetStudentById methodunu kullanarak
                                                                                 // göstericek ve bu methodun int türünde bir id'si olduğu için new'den sonra
                                                                                 // id'yi model.ID ile eşliyoruz atıyoruz.
           
        }





        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudent([FromBody] StudentDTO model) 
        {
            if (model == null || model.ID <= 0)
            {
                BadRequest();
            }
            var existingStudent = CollegeRepository.Students.Where(s=> s.ID == model.ID).FirstOrDefault();
            
            if (existingStudent == null)
            {
                NotFound();
            }

            existingStudent.StudentName = model.StudentName;
            existingStudent.Address = model.Address;
            existingStudent.Email = model.Email;

            return NoContent();
        }


        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudentPartial(int id,[FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
            {
                BadRequest();
            }
            var existingStudent = CollegeRepository.Students.Where(s => s.ID == id).FirstOrDefault();

            if (existingStudent == null)
            {
                NotFound();
            }
            var studentDTO = new StudentDTO
            {
                ID = existingStudent.ID,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };
            patchDocument.ApplyTo(studentDTO, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
 

            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.Address = studentDTO.Address;
            existingStudent.Email = studentDTO.Email;

            //204 - NoContent
            return NoContent();
        }




        //route constraint belirtmezsek alpha ve int diye api hangi methodu kullancağını bilemez ve hata verir

        [HttpGet("{name:alpha}", Name = "GetAllStudentByName")] //aplha means string
        [ProducesResponseType(StatusCodes.Status200OK)] //undocumented olmaması için
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  ActionResult<StudentDTO> GetStudentsByName(string name)
        {
            // BadRequest - 400 - Client Error
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var student = CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();
            // NotFound - 404 - Clint Error
            if (student == null)
            {
                return NotFound($"The student with id {name} not found");
            }

            var studentDTO = new StudentDTO()
            {
                ID = student.ID,
                StudentName = student.StudentName,
                Address = student.Address,
                Email = student.Email
            };
            return Ok(studentDTO);
        }





        [HttpDelete("{id:min(1):max(100)}", Name = "DeleteStudentById")]  //min max => route Constraints
        [ProducesResponseType(StatusCodes.Status200OK)] //undocumented olmaması için
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult <bool> DeleteStudent(int id)
        {
            // BadRequest - 400 - Client Error
            if (id <= 0)
            {
                return BadRequest();
            }

            var student = CollegeRepository.Students.Where(n => n.ID == id).FirstOrDefault();
            // NotFound - 404 - Clint Error
            if (student == null)
            {
                return NotFound($"The student with id {id} not found");
            }
            CollegeRepository.Students.Remove(student);

            // OK - 200 - Success
            return Ok(true);
        }

    }
}
