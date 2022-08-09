using AutoMapper;
using Musference.Data;
using Musference.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Musference.Models;
using Musference.Exceptions;

namespace Musference.Services
{
    public interface ITrackService
    {
        public IEnumerable<GetTrackDto> GetAllTrack();
        public int AddTrack(int id, AddTrackDto dto);
        public void DeleteTrack(int id);
        public void LikeTrack(int id);
        public void UnLikeTrack(int id);
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
        public IEnumerable<GetTrackDto> GetAllTrack()
        {
            List<GetTrackDto> trackListDto = new List<GetTrackDto>();
            var trackList = _context.QuestionsDbSet.ToList();
            foreach (var item in trackList)
            {
                var gettrackdto = _mapper.Map<GetTrackDto>(item);
                trackListDto.Add(gettrackdto);
            }
            return trackListDto;
        }
        public int AddTrack(int id, AddTrackDto dto)
        {
            var user = _context.UsersDbSet
                .Include(t=>t.Tracks)
                .FirstOrDefault(u=>u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var track = _mapper.Map<Track>(dto);
            user.Tracks.Add(track);
            _context.SaveChanges();
            return track.Id;
        }
        public void DeleteTrack(int id)
        {
            var track = _context.TracksDbSet.FirstOrDefault(t => t.Id == id);
            if (track == null)
            {
                throw new NotFoundException("Track not found");
            }
            var tracks = _context.TracksDbSet.ToList();
            tracks.Remove(track);
            _context.SaveChanges();
        }
        public void LikeTrack(int id)
        {
            var track = _context.TracksDbSet.FirstOrDefault(t => t.Id == id);
            if (track == null)
            {
                throw new NotFoundException("Track not found");
            }
            track.Likes++;
        }
        public void UnLikeTrack(int id)
        {
            var track = _context.TracksDbSet.FirstOrDefault(t => t.Id == id);
            if (track == null)
            {
                throw new NotFoundException("Track not found");
            }
            if (track.Likes>0)
                track.Likes--;
        }
    }
}
