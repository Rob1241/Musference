using AutoMapper;
using Musference.Data;
using Musference.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Musference.Models;
using Musference.Exceptions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Musference.Models.EndpointModels;

namespace Musference.Services
{
    public interface ITrackService
    {
        public Task<List<GetTrackDto>> SearchTrack(string text);
        public Task<TrackResponse> GetAllTrackNewest(int page);
        public Task<TrackResponse> GetAllTrackBestUsers(int page);
        public Task<int> AddTrack(int id, AddTrackDto dto);
        public void DeleteTrack(int id, int userId);
        public void LikeTrack(int id, int userId);
        //public void ReportTrack(int id, int userId, string reason);
        //public void UnLikeTrack(int id);
    }
    public class TrackService : ITrackService
    {
        private readonly IMapper _mapper;
        private readonly DataBaseContext _context;

        public TrackService(DataBaseContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<GetTrackDto>> SearchTrack(string text)
        {
            List<GetTrackDto> trackListDto = new List<GetTrackDto>();
            var listToReturn = await _context.TracksDbSet
                                        .Where(c => (c.Artist.ToLower().Contains(text.ToLower()))
                                        || c.Title.ToLower().Contains(text.ToLower()))
                                        .ToListAsync();
            foreach (var item in listToReturn)
            {
                var gettrackdto = _mapper.Map<GetTrackDto>(item);
                trackListDto.Add(gettrackdto);
            }
            return trackListDto;
        }
        public async Task<TrackResponse> GetAllTrackNewest(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.TracksDbSet.Count() / pageResults);
            var sortedtrack = await _context.TracksDbSet.OrderBy(t => t.DateAdded).ToListAsync();
            var tracks = sortedtrack
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetTrackDto> trackListDto = new List<GetTrackDto>();
            foreach (var item in tracks)
            {
                var gettrackdto = _mapper.Map<GetTrackDto>(item);
                trackListDto.Add(gettrackdto);
            }

            var response = new TrackResponse
            {
                Tracks = trackListDto,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }
        public async Task<TrackResponse> GetAllTrackBestUsers(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.TracksDbSet.Count() / pageResults);
            var sortedtrack = await _context.TracksDbSet.OrderBy(t => t.User.Reputation).ToListAsync();
            var tracks = sortedtrack
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetTrackDto> trackListDto = new List<GetTrackDto>();
            foreach (var item in tracks)
            {
                var gettrackdto = _mapper.Map<GetTrackDto>(item);
                trackListDto.Add(gettrackdto);
            }

            var response = new TrackResponse
            {
                Tracks = trackListDto,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }
        public async Task<int> AddTrack(int userId, AddTrackDto dto)
        {
            var user = await _context.UsersDbSet
                .Include(t=>t.Tracks)
                .FirstOrDefaultAsync(u=>u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var track = _mapper.Map<Track>(dto);
            track.Artist = user.Name;
            track.User = user;
            //user.Tracks.Add(track);
            await _context.SaveChangesAsync();
            return track.Id;
        }
        public async void DeleteTrack(int id, int userId)
        {
            var track = await _context.TracksDbSet.FirstOrDefaultAsync(t => t.Id == id);
            if (track == null)
            {
                throw new NotFoundException("Track not found");
            }
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (track.User.Id == user.Id)
            {
                var tracks = await _context.TracksDbSet.ToListAsync();
                tracks.Remove(track);
                await _context.SaveChangesAsync();
            }
            //tu jakis error
            
        }
        public async void LikeTrack(int id, int userId)
        {
            var track = await _context.TracksDbSet.FirstOrDefaultAsync(t => t.Id == id);
            if (track == null)
            {
                throw new NotFoundException("Track not found");
            }
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var usersliked = track.UsersThatLiked;
            if (usersliked != null)
            {
                foreach (User userloop in usersliked)
                {
                    if (userloop.Id == userId)
                    {
                        //Tu jakis blad
                    }
                }
            }
            user.TracksLiked.Add(track);
            track.Likes++;
            await _context.SaveChangesAsync();
        }
        //public void UnLikeTrack(int id)
        //{
        //    var track = _context.TracksDbSet.FirstOrDefault(t => t.Id == id);
        //    if (track == null)
        //    {
        //        throw new NotFoundException("Track not found");
        //    }
        //    if (track.Likes>0)
        //        track.Likes--;
        //}
    }
}
