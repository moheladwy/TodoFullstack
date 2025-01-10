using Todo.Core.DTOs.ListDTOs;
using Todo.Core.Entities;

namespace Todo.Api.Helpers;

public static class ListsHelper
{
  public static ListsDto MapToListsDto(TaskList list)
  {
    return new ListsDto
    {
      Id = list.Id,
      Name = list.Name,
      Description = list.Description,
    };
  }

  public static IEnumerable<ListsDto> MapToListsDto(IEnumerable<TaskList> lists) =>
    lists.Select(MapToListsDto);

  public static TaskList MapToTaskList(ListsDto list)
  {
    return new TaskList
    {
      Id = list.Id,
      Name = list.Name,
      Description = list.Description
    };
  }

  public static IEnumerable<TaskList> MapToTaskList(IEnumerable<ListsDto> lists) =>
    lists.Select(MapToTaskList);

  public static TaskList MapToTaskList(AddListDto list)
  {
    return new TaskList
    {
      Name = list.Name,
      Description = list.Description,
    };
  }
}