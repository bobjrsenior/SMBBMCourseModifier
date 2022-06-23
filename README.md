# SMBBM Course Modifier

A plugin for Super Monkey Ball Banana Mania designed to allow modifying the existing courses. With it you can completely change what stages go in each course (SMB1 Casual, Story Mode World 1, etc).

This plugin supports BOTH Banana Mod Manager (BMM) and BepInEx.

General Feature List:
1. Change Stages in a course
2. Roughly determine the halfway point where extra levels begin
3. (Affects Story Only) Determine what move should play before the course begins and after it ends
4. Change the "next_step" for goals (i.e. should the blue goal on stage 1 send you to stage 2 or 15?)
5. (BepInEx Only for now) Change the "next_step" for goals to 0 or a negative number

Disclaimers:
1. Backup your save file so you don't mess up your local times
2. (BepInEx Only for now) Making the goal with a next_step negative enough to go to stage 0 (or a negative stage) soft locks the game

## Installing for Banana Mod Manager

1. Backup your save file so you don't mess up your local times
2. Download the SMBBMCourseModifier.BMM.zip file from the Releases page
3. Extract it in your main game folder (the zip file structure should put the plugin in the right place)
4. Enable it within BMM

## Installing for BepInEx (Required if running through wine/proton)

### Installing BepInEx

This plugin uses [BepInEx](https://github.com/BepInEx/BepInEx) as a mod loader so that needs to be installed first.

1. Download a bleeding edge build of "BepInEx Unity IL2CPP for Windows x64 games" [here](https://builds.bepinex.dev/projects/bepinex_be) (Only the bleeding edge builds support Il2CPP games which is what Banana Mania is)
2. Extract it in your game folder)

### Install Plugin Dependencies

1. Install [SMBBM Leaderboard Disabler](https://github.com/bobjrsenior/SMBBMLeaderboardDisabler/releases)
2. Install [JsonLibs](https://github.com/bobjrsenior/JsonLibs/releases)

### Installing This Plugin

1. Backup your save file so you don't mess up your local times
2. Download the SMBBMCourseModifier.BepInEx.zip file from the Releases page
3. Extract it in your main game folder (the zip file structure should put the plugin in the right place)

## Using The Plugin

This Plugin uses JSON files to configure what courses should be modified. The Plugin looks for these JSON files under the "UserData/CourseDefinitions" directory within your games install folder.

It is good practice to name your configuration file after your custom file pack.

For example, if you wanted a custom Asset Bundle pack called "MyAwesomeSmb1CasualCourse", you could make your configuration file "MyAwesomeSmb1CasualCourse.json".

## JSON Configuration

The JSON configuration file is what determines the Courses to modify. The Plugin supports having multiple configurations so you can have multiple modified courses installed at once (although having multiple configurations for the same course will make one override the other).

The configuration file format looks like this:
```json
{
  "name": "SMB1 Casual Abridged",
  "description": "An abridged version of SMB1 casual for example purposes",
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
          "stage_id": 1001,
          "goals": [
            {
              "goal_kind": "Blue",
              "next_step": 1
            }
          ]
        },
        {
          "is_check_point": true,
          "is_half_time": true,
          "stage_id": 1103,
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

For more examples and descriptions of the format, see [Examples](/Examples).

Default Goal next_step values if non are provided:
1. (BMM Currently) If a Goal Color isn't defined, the next_step value defaults to 1
2. (BepInEx Only for now) If a Goal Color isn't defined, the default next_step value is 1 for Blue, 2 for Green, and 3 for Red 

Notes:
1. In the default courses, Green can technically go up to 4 but the next_step is generally 2
2. In the default courses, Red can technically go up to 7 and is 2 on one level but the next_step is generally 3

## Default Course Definitions

When making custom courses, knowing what the default ones look like is helpful. This repo has a list of the default courses for convenience. You can find them [here](/Default_Course_Defs.json)

Disclaimer: Some default courses use a next_step value of 0. Unless you really want a value of 0 use 1 in your custom course instead. The BepInEx version (and hopefully BMM version eventually) supports 0 and negative next_step values so you risk making an unbeatable course if you leave it at 0. The mod avoids this when loading default courses but can't for custom ones since it could be intentional.

## Building

## Setup

I use Visual Studio 2022  for development although I beleive it can also be compiled via command line. Additionally, make sure you setup your enviroment for BepInEx plugin development if building for BepInEx: https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/1_setup.html

## Configuration

In the .csproj, there is an element called `<SMBBMDir>` and `<SMBDirBep>`. If building for BMM, you should edit `<SMBBMDir>` to point to your game installation where BMM is installed. If building for BepInEx, you should edit `<SMBBMDir>` to point to your game installation where BMM is installed. The project references are determined based on that.

There are 2 Visual Studio build configurations:
1. Release_BMM: Builds a DLL for Banana Mod Manager
2. Release_BepInEx: Builds a DLL for BepInEx

Make sure you use the right configuration for your mod loader.
