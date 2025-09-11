using Application.Activities.Commands;
using Application.Activities.DTO;
using Application.Activities.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> GetActivities()
        {
            return await Mediator.Send(new GetActivityList.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActvityDetail(string id)
        {
            return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateActivity(CreateActivityDTO activityDTO)
        {
            return HandleResult(await Mediator.Send(new CreateActivity.Command { ActivityDTO = activityDTO }));
        }

        [HttpPut]
        public async Task<IActionResult> EditActivity(EditActivityDto activity)
        {
            return HandleResult(await Mediator.Send(new EditActivity.Command { ActivityDTO = activity }));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(string id)
        {
            return HandleResult(await Mediator.Send(new DeleteActivity.Command { Id = id }));
        }


        [HttpPost("{id}/attend")]
        public async Task<ActionResult> Attend(string id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command { Id = id }));
        }
    }
}
