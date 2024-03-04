# open-edit-ai

[Devpost](https://devpost.com/software/openeditai)

## Built with
`C#` `Python` `OpenAI API` `gpt-3.5-turbo` `WhisperAI` `FFmpeg` `WPF` `.NET`

## Inspiration
In today's fast-paced world, AI is paving the way for a new era. I'm jumping on board, eager to let it take care of the dull stuff so we can focus on what truly matters: making the most of our time.

## What it does
OpenEditAI revolutionizes video editing by utilizing cutting-edge AI technology. Simply upload your raw footage and provide a prompt outlining your vision. OpenEditAI then intelligently analyzes your content and applies creative editing techniques to transform it into a polished masterpiece. Say goodbye to tedious editing tasks and hello to effortless creativity with OpenEditAI.

## How we built it
I crafted OpenEditAI using a combination of robust technologies. The core engine is powered by C# .NET 8.0 WPF, providing a sturdy foundation for seamless user interaction. Leveraging the capabilities of Python, I integrated cutting-edge AI technologies, including OpenAI's Chat-GPT 3.5 Turbo for intelligent decision-making and OpenWhisper for dynamic subtitle generation. Additionally, I utilized the versatile ffmpeg library for efficient manipulation of media files, ensuring a smooth and comprehensive editing experience. 

## Challenges we ran into
Setting up the project proved challenging, particularly in configuring ffmpeg and familiarizing myself with its functions and commands. I also faced difficulties with hallucination and performance issues while running OpenWhisperAI locally on my computer, prompting me to opt for OpenAI due to its consistency and robustness. Deadlocks arose when managing multiple threads for rapid editing. Time constraints limited my ability to implement AI-driven decision-making based on video frames. Additionally, interacting with OpenAI presented obstacles, as there is no official support for a C# NuGet package, necessitating the use of Python. In addition, I was limited by time and token generation which limited the total video time its able to edit by only 5 minutes for any video.

## Accomplishments that we're proud of
I'm thrilled to have achieved the capability to edit videos based on audio context and customize scene selection with prompts. It's a significant milestone in making video editing more intuitive and efficient, empowering users to express their creativity effortlessly. And the ability to edit a video very quickly.

## What we learned
Through this project, I gained valuable experience in utilizing OpenAI's API, leveraging GitHub Copilot for enhanced coding assistance, and exploring the versatile capabilities of the ffmpeg library, widely used across various industries. Additionally, I honed my skills in processing data with multithreading and navigating the interaction between two different programming languages, C# and Python.

## What's next for OpenEditAI
The future of OpenEditAI looks promising, with plans to introduce support for multiple videos, enhanced token capabilities, and extended editing capabilities for longer videos. Additionally, we aim to implement scene selection based on visual context, further streamlining the editing process and unlocking new creative possibilities.
