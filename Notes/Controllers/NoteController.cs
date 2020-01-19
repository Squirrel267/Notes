using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Notes.Controllers
{
    [Authorize]
    public class NoteController: Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly UserContext _context;


        public NoteController(UserManager<User> userManager,  UserContext context)
        {
            _userManager = userManager;
            _context = context;

        }

        private async Task<User> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet]
        public IActionResult AddNote()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNote(Note notes)
        {

            if (ModelState.IsValid)
            {
                string id = _userManager.GetUserId(User);
                notes.UserId = Guid.Parse(id);
                _context.Note.Add(notes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(notes);
        }
        public IActionResult ListOfNotes(string searchString)
        {
            string userId = _userManager.GetUserId(User);
            List<Note> userNotes;
            if (!String.IsNullOrEmpty(searchString))
            {
                userNotes = _context.Note.Where(x => x.UserId == Guid.Parse(userId) && (x.Content.Contains(searchString) || x.Header.Contains(searchString))).ToList();
            }
            else {
                userNotes = _context.Note.Where(x => x.UserId == Guid.Parse(userId)).ToList();

            }
            return View( userNotes);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Note note = _context.Note.FirstOrDefault(n => n.Id == id);
            return View(note);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Note note)
        {

            string userId = _userManager.GetUserId(User);
            note.UserId = Guid.Parse(userId);

            if (note != null)
            {
                _context.Note.Update(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
        public async Task<IActionResult> Delete (int id)
        {
            Note note = _context.Note.FirstOrDefault(n => n.Id == id);
            if (note != null)
            {
                _context.Note.Remove(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }


    }
}
