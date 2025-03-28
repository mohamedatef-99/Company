using Company.DAL.Models;
using Company.PL.Dtos;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManger;

        public UserController(UserManager<AppUser> userManger)
        {
            _userManger = userManger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturn> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
               users = _userManger.Users.ToList().Select(U => new UserToReturn()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManger.GetRolesAsync(U).Result
                });
            }
            else
            {
                users = _userManger.Users.ToList().Select(U => new UserToReturn()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManger.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var user = await _userManger.FindByIdAsync(id);
            if (user is null) return NotFound(new { statusCode = 404, message = "User Not Found" });
            var dto = new UserToReturn()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = _userManger.GetRolesAsync(user).Result
            };
            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturn model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation");
                var user = await _userManger.FindByIdAsync(id);
                if(user is null) return BadRequest("Invalid Operation");
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;


                var result = await _userManger.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserToReturn model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation");
                var user = await _userManger.FindByIdAsync(id);
                if (user is null) return BadRequest("Invalid Operation");

                var result = await _userManger.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
