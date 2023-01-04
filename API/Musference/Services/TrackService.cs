using AutoMapper;
using Musference.Data;
using Musference.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Musference.Exceptions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Musference.Logic;
using CloudinaryDotNet.Actions;
using Musference.Models.EndpointModels.Track;
using Musference.Models.Entities;

namespace Musference.Services
{
    public interface ITrackService
    {
        public Task<TrackResponse> SearchTrack(string text, int page);
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
        private readonly IPagination _pagination;

        public TrackService(DataBaseContext context, IMapper mapper,IPagination pagination)
        {
            _mapper = mapper;
            _context = context;
            _pagination = pagination;
        }
        public async Task<TrackResponse> SearchTrack(string text, int page)
        {
            var pageResults = 10f;
            var listToReturn = await _context.TracksDbSet
                                        .Where(c => (c.Artist.ToLower().Contains(text.ToLower()))
                                        || c.Title.ToLower().Contains(text.ToLower()))
                                        .ToListAsync();
            var pageCount = Math.Ceiling(listToReturn.Count() / pageResults);
            var response = _pagination.TrackPagination(listToReturn, pageResults, page, pageCount);
            return response;
        }
        public async Task<TrackResponse> GetAllTrackNewest(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.TracksDbSet.Count() / pageResults);
            var sortedtrack = await _context.TracksDbSet.OrderBy(t => t.DateAdded).ToListAsync();
            var response = _pagination.TrackPagination(sortedtrack, pageResults, page, pageCount);
            return response;
        }
        public async Task<TrackResponse> GetAllTrackBestUsers(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.TracksDbSet.Count() / pageResults);
            var sortedtrack = await _context.TracksDbSet.OrderBy(t => t.User.Reputation).ToListAsync();
            var response = _pagination.TrackPagination(sortedtrack, pageResults, page, pageCount);
            return response;
        }
        public async Task<int> AddTrack(int userId, AddTrackDto dto)
        {
            var user = await _context.UsersDbSet
                .Include(u=>u.Tracks)
                .FirstOrDefaultAsync(u=>u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var track = _mapper.Map<Track>(dto);
            track.Artist = user.Name;
            await _context.TracksDbSet.AddAsync(track);
            user.Tracks.Add(track);
            await _context.SaveChangesAsync();
            return track.Id;
        }
        public async void DeleteTrack(int id, int userId)
        {
            var track = await _context.TracksDbSet.FirstOrDefaultAsync(t => t.Id == id);
            if (track == null)
                throw new NotFoundException("Track not found");
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) 
                throw new NotFoundException("User not found");
            if (track.User.Id != user.Id)
            {
                throw new UnauthorizedException("Unauthorized");
            }
            _context.TracksDbSet.Remove(track);
            await _context.SaveChangesAsync();

        }
        public async void LikeTrack(int id, int userId)
        {
            var track = await _context.TracksDbSet
                .Include(t=>t.UsersThatLiked)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (track == null)
            throw new NotFoundException("Track not found");
            var user = await _context.UsersDbSet
                .Include(u=>u.TracksLiked)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            throw new NotFoundException("User not found");
            var trackowner= await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == track.UserId);
            var usersliked = track.UsersThatLiked;
            if (trackowner == null)
            throw new NotFoundException("User not found");
            if (usersliked != null)
            {
                foreach (User userloop in usersliked)
                {
                    if (userloop.Id == userId)
                        throw new NotFoundException("You can like only once");
                }
            }
            user.TracksLiked.Add(track);
            track.Likes++;
            trackowner.Reputation++;
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
