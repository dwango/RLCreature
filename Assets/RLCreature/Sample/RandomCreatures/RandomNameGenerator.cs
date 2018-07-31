using System.Linq;
using UnityEngine;

namespace RLCreature.Sample.RandomCreatures
{
    public class RandomNameGenerator
    {
        // this list is derived from docker names generator https://github.com/moby/moby/blob/master/pkg/namesgenerator/names-generator.go
        // Copyright 2013-2017 Docker, Inc.
        private static readonly string[] Adjectives =
        {
            "admiring",
            "adoring",
            "affectionate",
            "agitated",
            "amazing",
            "angry",
            "awesome",
            "blissful",
            "boring",
            "brave",
            "clever",
            "cocky",
            "compassionate",
            "competent",
            "condescending",
            "confident",
            "cranky",
            "dazzling",
            "determined",
            "distracted",
            "dreamy",
            "eager",
            "ecstatic",
            "elastic",
            "elated",
            "elegant",
            "eloquent",
            "epic",
            "fervent",
            "festive",
            "flamboyant",
            "focused",
            "friendly",
            "frosty",
            "gallant",
            "gifted",
            "goofy",
            "gracious",
            "happy",
            "hardcore",
            "heuristic",
            "hopeful",
            "hungry",
            "infallible",
            "inspiring",
            "jolly",
            "jovial",
            "keen",
            "kind",
            "laughing",
            "loving",
            "lucid",
            "mystifying",
            "modest",
            "musing",
            "naughty",
            "nervous",
            "nifty",
            "nostalgic",
            "objective",
            "optimistic",
            "peaceful",
            "pedantic",
            "pensive",
            "practical",
            "priceless",
            "quirky",
            "quizzical",
            "relaxed",
            "reverent",
            "romantic",
            "sad",
            "serene",
            "sharp",
            "silly",
            "sleepy",
            "stoic",
            "stupefied",
            "suspicious",
            "tender",
            "thirsty",
            "trusting",
            "unruffled",
            "upbeat",
            "vibrant",
            "vigilant",
            "vigorous",
            "wizardly",
            "wonderful",
            "xenodochial",
            "youthful",
            "zealous",
            "zen",
        };


        private static readonly string[] Nouns = new string[]
        {
            "cat",
            "dog",
            "creature",
            "parrot",
            "horse",
            "bug",
            "snake",
            "gorilla",
            "bird",
            "squirrel",
            "bear",
        };

        public static string EnglishName()
        {
            return Adjectives[Random.Range(0, Adjectives.Length)] + "_" + Nouns[Random.Range(0, Nouns.Length)];
        }

        public static string JapaneseName()
        {
            const string charas = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん";
            return Enumerable.Range(0, Random.Range(3, 7))
                .Select(_ => charas[Random.Range(0, charas.Length)].ToString()).Aggregate((x, y) => x + y);
        }
    }
}