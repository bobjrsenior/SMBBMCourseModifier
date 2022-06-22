# Examples Walkthrough

This folder contains 3 example JSON configuration files: SMB1 Casual Backwards.json, Simple Story World 1.json, and SMB ReZero.json.

## SMB1 Casual Backwards.json

To install this example, copy "SMB1 Casual Backwards.json" to "UserData/CourseDefinitions" in your game directory.

The idea of this example is simple. It's SMB1 Casual but you start at what's normally the last stage and work your way to what's normally the first stage.

```json
{
    "name": "SMB1 Casual Backwards",
    "description": "Do you think you know the SMB1 Casual course fowards and backwards? Now's your chance to find out!",
    "author": "bobjrsenior",
    "file_format_version": 1,
    "course_defs": {
      "Smb1_Casual": {
        "next_course": "Invalid",
        "start_movie_id": "Invalid",
        "end_movie_id": "Invalid",
        "course_stages": [
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1103,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1102,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1101,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1009,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1008,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1007,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1006,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1005,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1091,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": true,
            "is_half_time": false,
            "stage_id": 1004,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": true,
            "stage_id": 1003,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": true,
            "stage_id": 1002,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              },
              {
                "goal_kind": "Green",
                "next_step": 3
              }
            ]
          },
          {
            "is_check_point": true,
            "is_half_time": true,
            "stage_id": 1001,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          }
        ]
      }
    }
  }
```
    
Going though the parts:

```json
    "name": "SMB1 Casual Backwards",
    "description": "Do you think you know the SMB1 Casual course fowards and backwards? Now's your chance to find out!",
    "author": "bobjrsenior",
    "file_format_version": 1,
```

name, description, and author are informational only. They aren't used by the mod but are listed to have the Course's description somewhere and to keep track of the author.

```json
"course_defs": {
      "Smb1_Casual": {
        "next_course": "Invalid",
        "start_movie_id": "Invalid",
        "end_movie_id": "Invalid",
```

course_defs is where things start. It's a dictionary. Every Key is a course name and the value is the course definition. For this example, the course is "Smb1_Casual".

next_course, start_movie_id, and end_movie_id are meant for Story mode.

- next_course: The course that should play after this one (ex: Story World 2 starts after Story World 1)
- start_movie_id: The movie that plays before you begin the course (i.e. The Story Mode cutscenes)
- end_movie_id: The movie that plays before you finish the course (i.e. The Story Mode cutscenes)

In this example, the next_course, start_movie_id, and end_movie_id values are all "Invalid". This just means there isn't a value defined since it's Story Mode only. You can also just not include the keys in the configuration at all but they are here for general reference.

```json
"course_stages": [
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1103,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 1102,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          ...
```

course_stages is the actual array of stages you want to be a part of the Course. The stages in the Course will be in the same order you include them in the course_stages array.

is_check_point and is_half_time have to do with extra stages. The default courses can be kind of inconsistent on how they use it. They can be used if you want an Extra Stages portion of your course.

stage_id is simply the id of the stage to play for this course.

```json
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
```

goals is an array of goal definitions for each stage. You can use this to change how many stages a goal should skip ahead.

goal_kind is the kind of Goal (either Blue, Green, or Red).

next_step is how many stages getting that goal should move you forward (1 being the next stage).

In BMM: Negative and 0 next_step values will be converted to 1 by the game. Any Goal type not defined will also become 1.
In BepInEx: Negative and 0 next_step values are allowed. The next_step for any Goal type not defined will be 1 for Blue, 2 for Green, and 3 for Red.

## Simple Story World 1.json

To install this example, copy "Simple Story World 1.json" to "UserData/CourseDefinitions" in your game directory.

The idea of this example is simpler than the last one. It take Story Mode World 1 and make every stage "Simple".

```json
{
    "name": "Simple Story World 1",
    "description": "Story Mode is complicated since there are so many choices. This makes world 1 Simple!",
    "author": "bobjrsenior",
    "file_format_version": 1,
    "course_defs": {
      "Smb2_StoryWorld01": {
        "next_course": "Smb2_StoryWorld02",
        "start_movie_id": "Story01_A",
        "end_movie_id": "Story01_B",
        "course_stages": [
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          },
          {
            "is_check_point": false,
            "is_half_time": false,
            "stage_id": 2201,
            "goals": [
              {
                "goal_kind": "Blue",
                "next_step": 1
              }
            ]
          }
        ]
      }
    }
  }
```

To avoid re-explaining things, only the unique part not fully covered by the SMB1 Casual Backwards.json explanation will be covered.

```json
        "next_course": "Smb2_StoryWorld02",
        "start_movie_id": "Story01_A",
        "end_movie_id": "Story01_B",
```

It was mentioned above but:

- next_course: The course that should play after this one (ex: Story World 2 starts after Story World 1)
- start_movie_id: The movie that plays before you begin the course (i.e. The Story Mode cutscenes)
- end_movie_id: The movie that plays before you finish the course (i.e. The Story Mode cutscenes)

In this example:
1. the movie Story01_A will play before this Course begins
2. The movie Story01_B will play after finishing the Course
3. The Course Smb2_StoryWorld02 will start after finishing this course

# SMB ReZero.json

To install this example, copy "SMB ReZero.json" to "UserData/CourseDefinitions" in your game directory.

Disclaimers:
1. Currently only works correctly in the BepInEx version
2. This course includes data for the SMB1 Casual Course which conflicts with "SMB1 Casual Backwards.json"

--- Configuration not included here due to size ---

The main things to note about this configuration are:
1. It makes use of negative next_step values for goals
2. It includes a Custom Course for every SMB1 and SMB2 Course in one configuration file

There's probably some math errors but the idea of the Course is that you start over if you don't go through the hardest goal.

The Rules basically go as:

- If there is only a Blue Goal
1. Reaching the Blue Goal makes you go to the next stage
- If there is a Green Goal and no Red Goal
1. Reaching the Blue Goal makes you go to the previous stage that had a Green Goal or Stage 1 if none exist
2. Reaching the Green Goal makes you go to the next stage
- If there is a Red Goal and no Blue Goal
1. Reaching the Blue Goal makes you go to the previous stage that had a Red Goal or Stage 1 if none exist
2. Reaching the Red Goal makes you go to the next stage
- If there is a Red Goal and Green Goal
1. Reaching the Blue Goal makes you go to the previous stage that had a Red Goal or Stage 1 if none exist
2. Reaching the Green Goal makes you go to the previous stage that had a Green Goal or Stage 1 if none exist
3. Reaching the Red Goal makes you go to the next stage
