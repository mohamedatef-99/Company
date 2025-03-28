using System.Threading.Tasks;
using Company.DAL.Models;
using Company.PL.Dtos;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManger, UserManager<AppUser> userManager)
        {
            _roleManger = roleManger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturn model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManger.FindByNameAsync(model.Name);
                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name
                    };
                    var result = await _roleManger.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
                return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturn> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManger.Roles.ToList().Select(U => new RoleToReturn()
                {
                    Id = U.Id,
                    Name = U.Name
                });
            }
            else
            {
                roles = _roleManger.Roles.ToList().Select(U => new RoleToReturn()
                {
                    Id = U.Id,
                    Name = U.Name
                }).Where(R => R.Name.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var role = await _roleManger.FindByIdAsync(id);
            if (role is null) return NotFound(new { statusCode = 404, message = "Role Not Found" });

            var dto = new RoleToReturn()
            {
                Id = role.Id,
                Name = role.Name
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
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturn model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation");
                var role = await _roleManger.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Operation");

                var roleResult = await _roleManger.FindByNameAsync(model.Name);
                if(roleResult is null)
                {
                    role.Name = model.Name;

                    var result = await _roleManger.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", "Invalid Operation");
                
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
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturn model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation");
                var role = await _roleManger.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Operation");

                var result = await _roleManger.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Invalid Operation");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _roleManger.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            ViewData["roleId"] = roleId;
            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach(var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if ( await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }

                usersInRole.Add(userInRole);
            }

            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManger.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            

            if (ModelState.IsValid) {

                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if(!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                    
                }
                return RedirectToAction(nameof(Edit), new {id = roleId});

            }
            return View(users);
        }
    }
}
