﻿using Api.Chat;
using Api.Data.Entities;
using Api.Data.NoSql.Contracts;
using Api.Data.Repositories.Contracts;
using Api.Infra;
using Api.Service.Contracts;
using Api.Utils.Constants;
using Api.ViewModels.Response;

namespace Api.Service.Implementations;

public class ActivityService : IActivityService
{
    private readonly ISocialRepository _socialRepository;

    public ActivityService(ISocialRepository socialRepository)
    {
        _socialRepository = socialRepository;
    }

    public async Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesByFilterPaginationAsync(int skip, int count, string key, DateTime fromDate, DateTime toDate, int fromCapacity, int toCapacity, List<string> categories)
    {
        var activities = await _socialRepository.GetActivityPaginationFilterAsync(skip, count, key, fromDate, toDate, fromCapacity, toCapacity, categories);

        return new()
        {
            IsSuccessful = true,
            Response = activities
        };
    }

    public async Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesRandomlyByFilterAsync(int count, string key, DateTime fromDate, DateTime toDate,
        int fromCapacity, int toCapacity, List<string> categories)
    {
        var activities = await _socialRepository.GetActivityRandomlyByFilterAsync(count, key, fromDate, toDate, fromCapacity, toCapacity, categories);
        
        return new()
        {
            IsSuccessful = true,
            Response = activities
        };
    }

    public async Task<ServiceResponse<IEnumerable<Activity>>> GetUserActivitiesAsync(int userId)
    {
        var activities = await _socialRepository.GetUserActivityAsync(userId);

        return new()
        {
            IsSuccessful = true,
            Response = activities
        };
    }

    public async Task<ServiceResponse<IEnumerable<Activity>>> GetOwnerActivitiesAsync(int userId)
    {
        var activities = await _socialRepository.GetOwnerActivityAsync(userId);

        return new()
        {
            IsSuccessful = true,
            Response = activities
        };
    }

    public async Task<ServiceResponse<ActivityDetailResponseModel>> GetActivityDetail(int activityId, int userId)
    {
        var activity = await _socialRepository.GetActivityByIdAsync(activityId);

        if (activity == null)
        {
            return new()
            {
                Error = ErrorMessages.NoRecordHasFound
            };
        }

        var joiners = await _socialRepository.GetUsersByActivityIdQueryAsync(activityId);

        joiners ??= new List<Profile>();

        var response = new ActivityDetailResponseModel()
        {
            Id = activityId,
            Date = activity.Date,
            UserId = userId,
            Detail = activity.Detail,
            Location = activity.Location,
            Title = activity.Title,
            PhoneNumber = activity.PhoneNumber,
            Capacity = activity.Capacity,
            Category = activity.Category,
            Joiners = joiners.Select(x => new UserResponseModel()
            {
                Id = x.Id,
                UserName = x.UserName
            }).ToList()
        };

        return new()
        {
            IsSuccessful = true,
            Response = response
        };
    }

    public async Task<ServiceResponse<bool>> JoinActivityAsync(int profileId, int activityId)
    {
        try
        {
            await _socialRepository.CreateProfileActivityAsync(activityId, profileId);
            return new()
            {
                IsSuccessful = true,
                Response = true
            };
        }
        catch (Exception ex)
        {
            // log ex
            return new()
            {
                Error = ErrorMessages.OperationHasFailed
            };
        }
    }

    public async Task<ServiceResponse<bool>> CreateActivityAsync(string title, string detail, string location, DateTime? date, string phoneNumber, int capacity, string category, int userId)
    {
        try
        {
            var activity = new Activity()
            {
                Title = title,
                Detail = detail,
                Location = location,
                Date = date,
                PhoneNumber = phoneNumber,
                OwnerProfileId = userId,
                Capacity = capacity,
                Category = category,
            };
            var createdActivity = await _socialRepository.CreateActivityAsync(activity);

            if ((createdActivity?.Id ?? 0) <= 0)
            {
                return new()
                {
                    Error = ErrorMessages.OperationHasFailed
                };
            }

            await _socialRepository.CreateProfileActivityAsync(createdActivity.Id, userId);

            return new()
            {
                IsSuccessful = true,
                Response = true
            };
        }
        catch (Exception ex)
        {
            // log ex
            return new()
            {
                Error = ErrorMessages.OperationHasFailed
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<Message>>> GetRoomMessages(int roomId)
    {
        try
        {
            var chatMessages = await _socialRepository.GetChatMessagesByActivityIdQueryAsync(roomId);

            var messages = chatMessages.Select(x => new Message
            {
                Author = new Author()
                {
                    Id = x.AuthorId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                },
                CreatedAt = x.CreatedAt,
                Status = x.Status,
                Text = x.Text,
                Type = x.Type,
                Id = x.MessageId,
                Height = x.Height,
                Width = x.Width,
                Name = x.ImageName,
                Uri = x.Uri,
                Size = x.Size
            });

            return new()
            {
                IsSuccessful = true,
                Response = messages
            };
        }
        catch (Exception ex)
        {
            // log ex
            return new()
            {
                Error = ErrorMessages.OperationHasFailed
            };
        }
    }
}
