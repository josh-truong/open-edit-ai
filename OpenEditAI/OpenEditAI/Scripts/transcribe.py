import os
import sys
import json
import argparse
from pathlib import Path
from openai import OpenAI
from config import config

def transcribe_audio(input_file):
    if not os.path.isfile(input_file):
        print(f"The input path {input_file} is not a file.")
        return

    output_file = generate_unique_filename(input_file)

    client = OpenAI(api_key=config.OPENAI_API_KEY)

    with open(input_file, "rb") as audio_file:
        transcription = client.audio.transcriptions.create(
            model="whisper-1", 
            file=audio_file,
            response_format="verbose_json",
            timestamp_granularities=["segment"]
        )

    with open(output_file, 'w+') as transcribed_file:
        json.dump(transcription.segments, transcribed_file)

    return output_file

def generate_unique_filename(input_file):
    counter = 0
    input_path = Path(input_file)
    output_file = input_path.with_name(input_path.stem + ".json")

    while os.path.isfile(output_file):
        counter += 1
        output_file = input_path.with_name(f"{input_path.stem}_{counter}.json")

    return output_file

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='Transcribe an audio file.')
    parser.add_argument('input_file', type=str, help='The path to the audio file to transcribe.')

    args = parser.parse_args()
    output_file = transcribe_audio(args.input_file)

    sys.stdout.write(str(output_file))