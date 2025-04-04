﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using YG;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance; //Instance to make is available in other scripts without reference

    [SerializeField] private string[] letter;
    [SerializeField] private GameObject gameComplete;
    //Scriptable data which store our questions data
    [SerializeField] private QuizDataScriptable questionDataScriptable;
    [SerializeField] private Image questionImage;           //картинка, которая в данный момент показана
    [SerializeField] private WordData[] answerWordList;     //список верных ответов
    [SerializeField] private WordData[] optionsWordList;    //list of options word in the game


    private GameStatus gameStatus = GameStatus.Playing;           //to keep track of game status
    [SerializeField] private string[] wordsArray = new string[12];    //массив, в котором хранится символ каждого параметра

    private List<int> selectedWordsIndex;                   //list which keep track of option word index w.r.t answer word index
    private int currentAnswerIndex = 0, currentQuestionIndex = 0;   //index to keep track of current answer and current question
    private bool correctAnswer = true;                      //bool to decide if answer is correct or not
    private string answerWord;                              //string to store answer of current question


    private int _saveData;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
     selectedWordsIndex = new List<int>();           //create a new list at start
        SetQuestion();                                  //set question
    }

    void SetQuestion()
    {
        gameStatus = GameStatus.Playing;                //set GameStatus to playing 

        //set the answerWord string variable
        answerWord = questionDataScriptable.questions[currentQuestionIndex].answer;
        //set the image of question
        questionImage.sprite = questionDataScriptable.questions[currentQuestionIndex].questionImage;
            
        ResetQuestion();                               //reset the answers and options value to orignal     

        selectedWordsIndex.Clear();                     //clear the list for new question
        Array.Clear(wordsArray, 0, wordsArray.Length);  //clear the array

        //добавляем корректные буквы в мпссив выбора букв
        for (int i = 0; i < answerWord.Length; i++)
        {
            wordsArray[i] = answerWord[i].ToString(); //char.ToUpper(answerWord[i]);
        }

        //затем добавляем рандомные буквы 
        for (int j = answerWord.Length; j < wordsArray.Length; j++)
        {
            wordsArray[j] = letter[Random.Range(0, 33)]; //(char)UnityEngine.Random.Range(65, 90); //UnityEngine.Random.Range(65, 90)
            print(wordsArray[j]);
        }

        wordsArray = ShuffleList.ShuffleListItems(wordsArray.ToList()).ToArray(); //Случайным образом перетасум массив букв

        //set the options words Text value
        for (int k = 0; k < optionsWordList.Length; k++)
        {
            optionsWordList[k].SetWord(wordsArray[k]);
        }

    }

    //Method called on Reset Button click and on new question
    public void ResetQuestion()
    {
        //activate all the answerWordList gameobject and set their word to "_"
        for (int i = 0; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(true);
            answerWordList[i].SetWord("_");
        }

        //Now deactivate the unwanted answerWordList gameobject (object more than answer string length)
        for (int i = answerWord.Length; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(false);
        }

        //activate all the optionsWordList objects
        for (int i = 0; i < optionsWordList.Length; i++)
        {
            optionsWordList[i].gameObject.SetActive(true);
        }

        currentAnswerIndex = 0;
    }

    /// <summary>
    /// When we click on any options button this method is called
    /// </summary>
    /// <param name="value"></param>
    public void SelectedOption(WordData value)
    {
        //if gameStatus is next or currentAnswerIndex is more or equal to answerWord length
        if (gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length) return;

        selectedWordsIndex.Add(value.transform.GetSiblingIndex()); //add the child index to selectedWordsIndex list
        value.gameObject.SetActive(false); //deactivate options object
        answerWordList[currentAnswerIndex].SetWord(value.wordValue); //set the answer word list

        currentAnswerIndex++;   //increase currentAnswerIndex

        //if currentAnswerIndex is equal to answerWord length
        if (currentAnswerIndex == answerWord.Length)
        {
            correctAnswer = true;   //default value
            //loop through answerWordList
            for (int i = 0; i < answerWord.Length; i++)
            {
                //if answerWord[i] is not same as answerWordList[i].wordValue
                if (answerWord[i].ToString() != answerWordList[i].wordValue)
                {
                    correctAnswer = false; //set it false
                    break; //and break from the loop
                }
            }

            //if correctAnswer is true
            if (correctAnswer)
            {
                Debug.Log("Correct Answer");
                gameStatus = GameStatus.Next; //set the game status
                currentQuestionIndex++; //increase currentQuestionIndex
                //if currentQuestionIndex is less that total available questions
                if (currentQuestionIndex < questionDataScriptable.questions.Count)
                {
                    Invoke("SetQuestion", 0.5f); //go to next question
                }
                else
                {
                    Debug.Log("Game Complete"); //else game is complete
                    gameComplete.SetActive(true);
                }
            }
        }
    }

    public void ResetLastWord()
    {
        if (selectedWordsIndex.Count > 0)
        {
            int index = selectedWordsIndex[selectedWordsIndex.Count - 1];
            optionsWordList[index].gameObject.SetActive(true);
            selectedWordsIndex.RemoveAt(selectedWordsIndex.Count - 1);

            currentAnswerIndex--;
            answerWordList[currentAnswerIndex].SetWord("_");
        }
    }

}

[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
}

public enum GameStatus
{
   Next,
   Playing
}
