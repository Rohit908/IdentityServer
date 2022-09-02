using Microsoft.AspNetCore.Mvc;
using PermissonBasedAuth.Constants;
using PermissonBasedAuth.CustomPolicyProvider;

namespace PermissonBasedAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController: Controller
    {
        [HasPermission(Permissions.Products.View)]
        [HttpGet("Read")]
        public IActionResult Read()
        {
            return Ok("Reading Successful");
        }

        [HasPermission(Permissions.Products.Create)]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return Ok("Creating Successful");
        }

        [HasPermission(Permissions.Products.Edit)]
        [HttpGet("Edit")]
        public IActionResult Edit()
        {
            return Ok("Editing Successful");
        }

        [HasPermission(Permissions.Products.Delete)]
        [HttpGet("Delete")]
        public IActionResult Delete()
        {
            return Ok("Deleting Successful");
        }
    }
}
