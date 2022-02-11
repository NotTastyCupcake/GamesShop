using GamesShop.Models.Store;
using GamesShop.Models.Store.SroreViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Data
{
    public class GameReposiroty : IRepositoryGame
    {
        private ApplicationContext _context;
        private IQueryable<Game> _games;

        public GameReposiroty(ApplicationContext context)
        {
            this._context = context;
        }

        public void Create(Game item)
        {
            _context.Add(item);
        }

        public void Create(Game item, string[] selectedGenres)
        {
            if (selectedGenres != null)
            {
                item.GenreAssigment = new List<GenreAssigment>();
                foreach (var genre in selectedGenres)
                {
                    var genreToAdd = new GenreAssigment { GameId = item.Id, GenreId = int.Parse(genre) };
                    item.GenreAssigment.Add(genreToAdd);
                }
            }
        }

        public async Task DeleteAsync(int? id)
        {
            var game = await GetItemAsync(id);
            _context.Games.Remove(game);
            await SaveAsync();
        }

        public async Task<Game> GetItemAsync(int? id)
        {
            var game = await GetItemsCollection()
                .FirstOrDefaultAsync(m => m.Id == id);
            return game;
        }

        public IQueryable<Game> GetItemsCollection()
        {
            _games = _context.Games
                .Include(d => d.Developer)
                .Include(g => g.GenreAssigment).ThenInclude(g => g.Genre)
                .AsNoTracking();
            return _games;

        }

        public IQueryable<Game> SearchItems(string gameName, int? idDeveloper, int? idGenre)
        {
            var games = GetItemsCollection();
            if (idDeveloper != null && idDeveloper != 0)
            {
                games = _games.Where(p => p.IdDeveloper == idDeveloper);
            }
            if (!String.IsNullOrWhiteSpace(gameName))
            {
                games = _games.Where(p => p.Name.Contains(gameName));
            }
            if (idGenre != null && idGenre != 0)
            {
                games = from game in _games
                         from ganre in game.GenreAssigment
                        where ganre.GenreId == idGenre
                        select game;
            }
            return games;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Game game)
        {

        }

        public void Update(Game gameToUpdate, string[] selectedGenres)
        {
            if (selectedGenres == null)
            {
                gameToUpdate.GenreAssigment = new List<GenreAssigment>();
                return;
            }

            var selectedGenresHS = new HashSet<string>(selectedGenres);
            var gameGenres = new HashSet<int>
                (gameToUpdate.GenreAssigment.Select(c => c.Genre.IdGenre));
            foreach (var genre in _context.Genres)
            {
                if (selectedGenresHS.Contains(genre.IdGenre.ToString()))
                {
                    if (!gameGenres.Contains(genre.IdGenre))
                    {
                        gameToUpdate.GenreAssigment.Add(new GenreAssigment { GameId = gameToUpdate.Id, GenreId = genre.IdGenre });
                    }
                }
                else
                {

                    if (gameGenres.Contains(genre.IdGenre))
                    {
                        GenreAssigment generToRemove = gameToUpdate.GenreAssigment.FirstOrDefault(i => i.GenreId == genre.IdGenre);
                        _context.Remove(generToRemove);
                    }
                }
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool ItemExist(int id)
        {
            return GetItemsCollection().Any(e => e.Id == id);
        }

    }
}
