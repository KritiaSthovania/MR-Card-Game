using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using UnityEngine.Networking;


static class Questions
{
    // The shuffled question array
    public static string[] questionArray;

    // The index of the current question in the question array
    public static int currentQuestionIndex;

    // The last index of the array
    public static int lastQuestionIndex;

    // The number of models
    public static int numberOfModels;

    // The number of models that have finished to load
    public static int numberOfModelsLoaded;

    // The path to the local folder on the device
    public static string pathToLevel;

    // If it is for android or not, to switch easily between editor and device testing
    public static bool androidBoot = false;
}

public class ActivateQuestions : MonoBehaviour
{
    // Define the "menus"
    public GameObject startMenu;
    public GameObject viewModel;
    public GameObject viewMultipleChoiceQuestion;
    public GameObject viewInputQuestion;

    // Define the children of the slider that permits to see all multiple choice answers
    public GameObject multipleChoice2Answers;

    // Define the slider object
    public GameObject multipleChoice3AnsersSlider;
    public GameObject multipleChoice4AnsersSlider;
    public GameObject multipleChoice5AnsersSlider;

    // Define the text objects, text field and buttons of the input question interface
    public TMP_Text questionNameInput;
    public TMP_Text questionTextInput;
    public Button confirmAnswerInput;
    public Button closeQuestionInput;
    public TMP_InputField answerFieldInput;
    public Button feedbackInput;

    // Define the text elements of the correct answer preview of the input question interface
    public GameObject previewCorrectAnswerInput;
    public TMP_Text yourWrongAnswer;
    public TMP_Text theCorrectAnswer;

    // Define the text objects and button of the multiple choice question interface
    public TMP_Text questionNameMC;
    public Button confirmAnswerMC;
    public Button closeQuestionMC;
    public Button feedbackMC;

    // Define the text objects and buttons of the preview with two answers
    public TMP_Text questionText2Answers;
    public Button answer12Answers;
    public Button answer22Answers;

    // Define the text objects and buttons of the preview with three answers
    public TMP_Text questionText3Answers;
    public Button answer13Answers;
    public Button answer23Answers;
    public Button answer33Answers;

    // Define the text objects and buttons of the preview with four answers
    public TMP_Text questionText4Answers;
    public Button answer14Answers;
    public Button answer24Answers;
    public Button answer34Answers;
    public Button answer44Answers;

    // Define the text objects and buttons of the preview with five answers
    public TMP_Text questionText5Answers;
    public Button answer15Answers;
    public Button answer25Answers;
    public Button answer35Answers;
    public Button answer45Answers;
    public Button answer55Answers;

    // Define two questions for correct and incorrect answers
    public Color correctColor;
    public Color incorrectColor;
    public Color typingColor;
    public Color selectedColor;
    public Color deselectedColor;
    public Color correctButtonColor;
    public Color incorrectButtonColor;

    // Define the target image objects
    public GameObject imageTarget1;
    public GameObject imageTarget2;
    public GameObject imageTarget3;
    public GameObject imageTarget4;
    public GameObject imageTarget5;

    // The begin of the url to the .obj object downloaded in the created examples
    public string urlBegin;

    // // The path to the question
    // public string questionPath;

    // The JSON Serialization for the input questions
    [Serializable]
    public class InputQuestion
    {
        public string exerciseType;
        public string name;
        public string question;
        public string answer;
        public int numberOfModels;
        public string model1Name;
        public string model2Name;
        public string model3Name;
        public string model4Name;
        public string model5Name;
    }

    // The JSON Serialization for the multiple choice questions
    [Serializable]
    public class MultipleChoiceQuestion
    {
        public string exerciseType;
        public string name;
        public string question;
        public int numberOfAnswers;
        public string answer1;
        public string answer2;
        public string answer3;
        public string answer4;
        public string answer5;
        public bool answer1Correct;
        public bool answer2Correct;
        public bool answer3Correct;
        public bool answer4Correct;
        public bool answer5Correct;
        public int numberOfModels;
        public string model1Name;
        public string model2Name;
        public string model3Name;
        public string model4Name;
        public string model5Name;
    }

    // The JSON Serialization for the Models
    [Serializable]
    public class Model
    {
        public string modelName;
        public int numberOfQuestionsUsedIn;
    }

    // The JSON Serialization for the log file
    [Serializable]
    public class Log
    {
        public int numberOfQuestions; // The number of already existing questions in the folder so that the new ones can be renamed
        public int numberOfModels; // The number of already existing model files in the folder so that the new ones can be renamed
        public string heading; // Heading of the description, name that users can give
        public string description; // The description text of the content / concepts that are needed for solving the exercises
    }

    // Start is called before the first frame update
    void Start()
    {
        // Disable the question menus
        viewMultipleChoiceQuestion.SetActive(false);
        viewInputQuestion.SetActive(false);

        // The number of models
        Questions.numberOfModels = 5;

        // The number of models that have finished to load
        Questions.numberOfModelsLoaded = 0;

        // Check if it is editor testing or android boot
        if(Questions.androidBoot == true)
        {
            // Android case
            Questions.pathToLevel = Path.Combine(Application.streamingAssetsPath, "Level2");

        } else {

            // Editor case
            Questions.pathToLevel = levelDirectoryPath;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method that import the .obj models of the current question and renders them on the image targets accordingly to their number
    public void DisplayModels()
    {
        // Get the current question path
        string questionPath = Questions.questionArray[Questions.currentQuestionIndex];

        // First Access the json string of the question file
        string json = File.ReadAllText(questionPath);

        // Remove all models (make them invisible, unbind them of their parent)
        RemoveAllModels();

        // Check what type of question it is
        if(json.Contains("input question") == true)
        {
            // Case it is an input question, extract the input question object
            InputQuestion question = JsonUtility.FromJson<InputQuestion>(json);

            // Check how many models there are
            if(question.numberOfModels == 1)
            {
                // Display one model
                AddModelOverTargetImage1(question.model1Name, questionPath);
                Debug.Log("one model added");

            } else if(question.numberOfModels == 2)
            {
                // Display two models
                AddModelOverTargetImage1(question.model1Name, questionPath);
                AddModelOverTargetImage2(question.model2Name, questionPath);
                Debug.Log("two models added");

            } else if(question.numberOfModels == 3)
            {
                // Display three models
                AddModelOverTargetImage1(question.model1Name, questionPath);
                AddModelOverTargetImage2(question.model2Name, questionPath);
                AddModelOverTargetImage3(question.model3Name, questionPath);
                Debug.Log("three models added");

            } else if(question.numberOfModels == 4)
            {
                // Display four models
                AddModelOverTargetImage1(question.model1Name, questionPath);
                AddModelOverTargetImage2(question.model2Name, questionPath);
                AddModelOverTargetImage3(question.model3Name, questionPath);
                AddModelOverTargetImage4(question.model4Name, questionPath);
                Debug.Log("four models added");

            } else if(question.numberOfModels == 5)
            {
                // Display five models
                AddModelOverTargetImage1(question.model1Name, questionPath);
                AddModelOverTargetImage2(question.model2Name, questionPath);
                AddModelOverTargetImage3(question.model3Name, questionPath);
                AddModelOverTargetImage4(question.model4Name, questionPath);
                AddModelOverTargetImage5(question.model5Name, questionPath);
                Debug.Log("five models added");
            }

        } else {

            // Case it is a multiple choice question, extract the multiple choice question object
            MultipleChoiceQuestion question = JsonUtility.FromJson<MultipleChoiceQuestion>(json);

            // Check how many models there are
            if(question.numberOfModels == 1)
            {
                // Display one model
                AddModelOverTargetImage1(question.model1Name, questionPath);
                Debug.Log("one model added");

            } else if(question.numberOfModels == 2)
            {
                // Display two models
                AddModelOverTargetImage1(question.model1Name, questionPath);
                AddModelOverTargetImage2(question.model2Name, questionPath);
                Debug.Log("two models added");

            } else if(question.numberOfModels == 3)
            {
                // Display three models
                AddModelOverTargetImage1(question.model1Name, questionPath);
                AddModelOverTargetImage2(question.model2Name, questionPath);
                AddModelOverTargetImage3(question.model3Name, questionPath);
                Debug.Log("three models added");

            } else if(question.numberOfModels == 4)
            {
                // Display four models
                AddModelOverTargetImage1(question.model1Name, questionPath);
                AddModelOverTargetImage2(question.model2Name, questionPath);
                AddModelOverTargetImage3(question.model3Name, questionPath);
                AddModelOverTargetImage4(question.model4Name, questionPath);
                Debug.Log("four models added");

            } else if(question.numberOfModels == 5)
            {
                // Display five models
                AddModelOverTargetImage1(question.model1Name, questionPath);
                AddModelOverTargetImage2(question.model2Name, questionPath);
                AddModelOverTargetImage3(question.model3Name, questionPath);
                AddModelOverTargetImage4(question.model4Name, questionPath);
                AddModelOverTargetImage5(question.model5Name, questionPath);
                Debug.Log("five models added");
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    // Method that delete and imports the model and displays them on the target images
    //-------------------------------------------------------------------------------------------------------------------------------------------

    // The method that deletes all childs (models) of the image target
    public void RemoveAllModels()
    {
        Debug.Log("All models deleted");
        // Destroy all children of the first target image
        foreach(Transform child in imageTarget1.transform)
        {
            // Set the model as child of the save model object
            child.parent = saveModelObject.transform;
        }

        // Destroy all children of the second target image
        foreach(Transform child in imageTarget2.transform)
        {
            // Set the model as child of the save model object
            child.transform.parent = saveModelObject.transform;
        }

        // Destroy all children of the third target image
        foreach(Transform child in imageTarget3.transform)
        {
            // Set the model as child of the save model object
            child.transform.parent = saveModelObject.transform;
        }

        // Destroy all children of the fourth target image
        foreach(Transform child in imageTarget4.transform)
        {
            // Set the model as child of the save model object
            child.transform.parent = saveModelObject.transform;      
        }

        // Destroy all children of the fifth target image
        foreach(Transform child in imageTarget5.transform)
        {
            // Set the model as child of the save model object
            child.transform.parent = saveModelObject.transform;
        }
    }

    // Import the first model and bind it to the first image target
    public async void AddModelOverTargetImage1(string name, string pathToQuestion)
    {
        // Access the model gameobject
        string json = File.ReadAllText(Path.GetDirectoryName(pathToQuestion) + @"\" + name);

        // Extract the gameobject
        Model model = JsonUtility.FromJson<Model>(json);

        // Get the name of the imported model (delete the ending '.json')
        string modelName = model.modelName.Substring(0, model.modelName.Length - 4);

        Debug.Log("Searching for game object: " + modelName);

        // Find the model in the children of the save model object
        GameObject obj = saveModelObject.transform.Find(modelName).gameObject;

        // Find the child object of the model
        GameObject childGameObject = obj.transform.GetChild(0).gameObject;

        // Access the box collider information of the child object
        BoxCollider m_Collider = childGameObject.GetComponent<BoxCollider>();

        // Get the position of the target image
        Vector3 position = imageTarget1.transform.position;

        // Get the scale
        Vector3 scaleVector = obj.transform.localScale;

        // Find the position over the target image where the model should be
        position = position + new Vector3(0, m_Collider.size.y/2 * scaleVector.x, 0);

        // Change the position of the model so that it stands over the marker
        obj.transform.position = position;

        // Set the model as child of the marker
        obj.transform.parent = imageTarget1.transform;
    }

    // Import the second model and bind it to the second image target
    public async void AddModelOverTargetImage2(string name, string pathToQuestion)
    {
        // Access the model gameobject
        string json = File.ReadAllText(Path.GetDirectoryName(pathToQuestion) + @"\" + name);

        // Extract the gameobject
        Model model = JsonUtility.FromJson<Model>(json);

        // Get the name of the imported model (delete the ending '.json')
        string modelName = model.modelName.Substring(0, model.modelName.Length - 4);

        Debug.Log("Searching for game object: " + modelName);

        // Find the model in the children of the save model object
        GameObject obj = saveModelObject.transform.Find(modelName).gameObject; 

        // Find the child object of the model
        GameObject childGameObject = obj.transform.GetChild(0).gameObject;

        // Access the box collider information of the child object
        BoxCollider m_Collider = childGameObject.GetComponent<BoxCollider>();

        // Get the position of the target image
        Vector3 position = imageTarget2.transform.position;

        // Get the scale
        Vector3 scaleVector = obj.transform.localScale;

        // Find the position over the target image where the model should be
        position = position + new Vector3(0, m_Collider.size.y/2 * scaleVector.x, 0);

        // Change the position of the model so that it stands over the marker
        obj.transform.position = position;

        // Set the model as child of the marker
        obj.transform.parent = imageTarget2.transform;
    }

    // Import the third model and bind it to the third image target
    public async void AddModelOverTargetImage3(string name, string pathToQuestion)
    {
        // Access the model gameobject
        string json = File.ReadAllText(Path.GetDirectoryName(pathToQuestion) + @"\" + name);

        // Extract the gameobject
        Model model = JsonUtility.FromJson<Model>(json);

        // Get the name of the imported model (delete the ending '.json')
        string modelName = model.modelName.Substring(0, model.modelName.Length - 4);

        Debug.Log("Searching for game object: " + modelName);

        // Find the model in the children of the save model object
        GameObject obj = saveModelObject.transform.Find(modelName).gameObject; 

        // Find the child object of the model
        GameObject childGameObject = obj.transform.GetChild(0).gameObject;

        // Access the box collider information of the child object
        BoxCollider m_Collider = childGameObject.GetComponent<BoxCollider>();

        // Get the position of the target image
        Vector3 position = imageTarget3.transform.position;

        /// Get the scale
        Vector3 scaleVector = obj.transform.localScale;

        // Find the position over the target image where the model should be
        position = position + new Vector3(0, m_Collider.size.y/2 * scaleVector.x, 0);

        // Change the position of the model so that it stands over the marker
        obj.transform.position = position;

        // Set the model as child of the marker
        obj.transform.parent = imageTarget3.transform;
    }

    // Import the fourth model and bind it to the fourth image target
    public async void AddModelOverTargetImage4(string name, string pathToQuestion)
    {
        // Access the model gameobject
        string json = File.ReadAllText(Path.GetDirectoryName(pathToQuestion) + @"\" + name);

        // Extract the gameobject
        Model model = JsonUtility.FromJson<Model>(json);

        // Get the name of the imported model (delete the ending '.json')
        string modelName = model.modelName.Substring(0, model.modelName.Length - 4);

        Debug.Log("Searching for game object: " + modelName);

        // Find the model in the children of the save model object
        GameObject obj = saveModelObject.transform.Find(modelName).gameObject; 

        // Find the child object of the model
        GameObject childGameObject = obj.transform.GetChild(0).gameObject;

        // Access the box collider information of the child object
        BoxCollider m_Collider = childGameObject.GetComponent<BoxCollider>();

        // Get the position of the target image
        Vector3 position = imageTarget4.transform.position;

        // Get the scale
        Vector3 scaleVector = obj.transform.localScale;

        // Find the position over the target image where the model should be
        position = position + new Vector3(0, m_Collider.size.y/2 * scaleVector.x, 0);

        // Change the position of the model so that it stands over the marker
        obj.transform.position = position;

        // Set the model as child of the marker
        obj.transform.parent = imageTarget4.transform;
    }

    // Import the fifth model and bind it to the fifth image target
    public async void AddModelOverTargetImage5(string name, string pathToQuestion)
    {
        // Access the model gameobject
        string json = File.ReadAllText(Path.GetDirectoryName(pathToQuestion) + @"\" + name);

        // Extract the gameobject
        Model model = JsonUtility.FromJson<Model>(json);

        // Get the name of the imported model (delete the ending '.json')
        string modelName = model.modelName.Substring(0, model.modelName.Length - 4);

        Debug.Log("Searching for game object: " + modelName);

        // Find the model in the children of the save model object
        GameObject obj = saveModelObject.transform.Find(modelName).gameObject; 

        // Find the child object of the model
        GameObject childGameObject1 = obj.transform.GetChild(0).gameObject;

        // Access the box collider information of the child object
        BoxCollider m_Collider = childGameObject1.GetComponent<BoxCollider>();

        // Get the position of the target image
        Vector3 position = imageTarget5.transform.position;

        // Get the scale
        Vector3 scaleVector = obj.transform.localScale;

        // Find the position over the target image where the model should be
        position = position + new Vector3(0, m_Collider.size.y/2 * scaleVector.x, 0);

        // Change the position of the model so that it stands over the marker
        obj.transform.position = position;

        // Set the model as child of the marker
        obj.transform.parent = imageTarget5.transform;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    // Helper methods for the method that activate the question interfaces depending on the current question
    //-------------------------------------------------------------------------------------------------------------------------------------------

    // Method that initializes the model, creates a box collider, resizes it, and sets it to the right position, sets the model as child of the parent
    public void InitializeModel(GameObject obj, GameObject parent)
    {
        // Get the child gamobject (there should always be one)
        // TODO check the number of children, and add one if needed
        GameObject childGameObject1 = obj.transform.GetChild(0).gameObject;

        // Add a box collider to the child
        childGameObject1.AddComponent<BoxCollider>();

        // Access the box collider information
        BoxCollider m_Collider = childGameObject1.GetComponent<BoxCollider>();

        // Get the greatest size of the sizes of the box collider
        float greatest = ReturnGreatestFloat(m_Collider.size.x, m_Collider.size.y, m_Collider.size.z);
        
        // Get the down scale factor you want
        float scale = (float)0.2 / greatest;

        // Down scale the model
        obj.transform.localScale = new Vector3(scale, scale, scale);

        // Get the position on which the model should be
        Vector3 position = parent.transform.position;
        position = position + new Vector3(0, m_Collider.size.y/2 * scale, 0);

        // Change the position of the model so that it stands over the marker
        obj.transform.position = position;

        // Set the model as child of the marker
        obj.transform.parent = parent.transform;
    }

    // Method that returns the greates number of the three floats given
    public float ReturnGreatestFloat(float size1, float size2, float size3)
    {
        // Initialize a size variable
        float greater = 0;

        // Check which one is greater from the two first sizes
        if(size1 >= size2)
        {
            greater = size1;
        } else {
            greater = size2;
        }

        // Check what is greater, the greatest of the two first sizes or the third size
        if(greater >= size3)
        {
            return greater;
        } else {
            return size3;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    // Method that activate the question interfaces depending on the current question
    //-------------------------------------------------------------------------------------------------------------------------------------------

    // Method that is executed when a user presses the "view question" button in the model view
    public void ActivateQuestion()
    {
        // Get the current question path
        string questionPath = Questions.questionArray[Questions.currentQuestionIndex];

        // Access the json string of the question file
        string json = File.ReadAllText(questionPath);

        // Disable the view model menu
        viewModel.SetActive(false);

        // Check if the path points at a multiple choice or input question
        if(json.Contains("input question") == true)
        {
            // Extract the object
            InputQuestion question = JsonUtility.FromJson<InputQuestion>(json);

            // The current question is an input question, open the right question UI
            viewInputQuestion.SetActive(true);

            // Display the question informations in the right text objects
            questionNameInput.text = question.name;
            questionTextInput.text = question.question;

            // Activate the confirm button and deactivate the close button
            closeQuestionInput.gameObject.SetActive(false);
            confirmAnswerInput.gameObject.SetActive(true);

            // Activate the input field and make it interactable)
            answerFieldInput.gameObject.SetActive(true);
            answerFieldInput.interactable = true;

            // Disable the correct answer preview
            previewCorrectAnswerInput.SetActive(false);

            // Set the right typing color to the input field
            GameObject.Find("TextAnswerInput").GetComponent<TMP_Text>().color = typingColor;

            // Empty the input field
            answerFieldInput.text = "";
            
        } else {

            // Extract the object
            MultipleChoiceQuestion question = JsonUtility.FromJson<MultipleChoiceQuestion>(json);

            // Enable the menu
            viewMultipleChoiceQuestion.SetActive(true);

            // Activate the confirm button and deactivate the close button
            closeQuestionMC.gameObject.SetActive(false);
            confirmAnswerMC.gameObject.SetActive(true);

            // Display the correct question name
            questionNameMC.text = question.name;

            // Check how many answers exist
            if(question.numberOfAnswers == 2)
            {
                // Enable the object that contains the question and two ansers
                multipleChoice2Answers.SetActive(true);

                // Disable the slider objects
                multipleChoice3AnsersSlider.SetActive(false);
                multipleChoice4AnsersSlider.SetActive(false);
                multipleChoice5AnsersSlider.SetActive(false);

                // Display the correct question and ansers
                questionText2Answers.text = question.question;
                answer12Answers.GetComponentInChildren<TMP_Text>().text = question.answer1;
                answer22Answers.GetComponentInChildren<TMP_Text>().text = question.answer2;

                // Make them interactable
                answer12Answers.interactable = true;
                answer22Answers.interactable = true;

                // Set the right button color
                answer12Answers.GetComponent<Image>().color = deselectedColor;
                answer22Answers.GetComponent<Image>().color = deselectedColor;

                // Set the right font color
                answer12Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                answer22Answers.GetComponentInChildren<TMP_Text>().color = typingColor;

            } else {

                // Disactivate the question and 2 answers group that does not use the slider
                multipleChoice2Answers.SetActive(false);

                if(question.numberOfAnswers == 3)
                {

                    // Activate the right slider with interface and deactivate the two others
                    multipleChoice3AnsersSlider.SetActive(true);
                    multipleChoice4AnsersSlider.SetActive(false);
                    multipleChoice5AnsersSlider.SetActive(false);

                    // Reset the position of the scroll rect
                    multipleChoice3AnsersSlider.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;

                    // Display the correct question and ansers
                    questionText3Answers.text = question.question;
                    answer13Answers.GetComponentInChildren<TMP_Text>().text = question.answer1;
                    answer23Answers.GetComponentInChildren<TMP_Text>().text = question.answer2;
                    answer33Answers.GetComponentInChildren<TMP_Text>().text = question.answer3;

                    // Make them interactable
                    answer13Answers.interactable = true;
                    answer23Answers.interactable = true;
                    answer33Answers.interactable = true;

                    // Set the right button color
                    answer13Answers.GetComponent<Image>().color = deselectedColor;
                    answer23Answers.GetComponent<Image>().color = deselectedColor;
                    answer33Answers.GetComponent<Image>().color = deselectedColor;

                    // Set the right font color
                    answer13Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer23Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer33Answers.GetComponentInChildren<TMP_Text>().color = typingColor;

                } else if(question.numberOfAnswers == 4) 
                {

                    // Activate the right slider with interface and deactivate the two others
                    multipleChoice3AnsersSlider.SetActive(false);
                    multipleChoice4AnsersSlider.SetActive(true);
                    multipleChoice5AnsersSlider.SetActive(false);

                    // Reset the position of the scroll rect
                    multipleChoice4AnsersSlider.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;

                    // Display the correct question and ansers
                    questionText4Answers.text = question.question;
                    answer14Answers.GetComponentInChildren<TMP_Text>().text = question.answer1;
                    answer24Answers.GetComponentInChildren<TMP_Text>().text = question.answer2;
                    answer34Answers.GetComponentInChildren<TMP_Text>().text = question.answer3;
                    answer44Answers.GetComponentInChildren<TMP_Text>().text = question.answer4;

                    // Make them interactable
                    answer14Answers.interactable = true;
                    answer24Answers.interactable = true;
                    answer34Answers.interactable = true;
                    answer44Answers.interactable = true;

                    // Set the right button color
                    answer14Answers.GetComponent<Image>().color = deselectedColor;
                    answer24Answers.GetComponent<Image>().color = deselectedColor;
                    answer34Answers.GetComponent<Image>().color = deselectedColor;
                    answer44Answers.GetComponent<Image>().color = deselectedColor;

                    // Set the right font color
                    answer14Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer24Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer34Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer44Answers.GetComponentInChildren<TMP_Text>().color = typingColor;

                } else {

                    // Activate the right slider with interface and deactivate the two others
                    multipleChoice3AnsersSlider.SetActive(false);
                    multipleChoice4AnsersSlider.SetActive(false);
                    multipleChoice5AnsersSlider.SetActive(true);

                    // Reset the position of the scroll rect
                    multipleChoice5AnsersSlider.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;

                    // Display the correct question and ansers
                    questionText5Answers.text = question.question;
                    answer15Answers.GetComponentInChildren<TMP_Text>().text = question.answer1;
                    answer25Answers.GetComponentInChildren<TMP_Text>().text = question.answer2;
                    answer35Answers.GetComponentInChildren<TMP_Text>().text = question.answer3;
                    answer45Answers.GetComponentInChildren<TMP_Text>().text = question.answer4;
                    answer55Answers.GetComponentInChildren<TMP_Text>().text = question.answer5;

                    // Make them interactable
                    answer15Answers.interactable = true;
                    answer25Answers.interactable = true;
                    answer35Answers.interactable = true;
                    answer45Answers.interactable = true;
                    answer55Answers.interactable = true;

                    // Set the right button color
                    answer15Answers.GetComponent<Image>().color = deselectedColor;
                    answer25Answers.GetComponent<Image>().color = deselectedColor;
                    answer35Answers.GetComponent<Image>().color = deselectedColor;
                    answer45Answers.GetComponent<Image>().color = deselectedColor;
                    answer55Answers.GetComponent<Image>().color = deselectedColor;

                    // Set the right font color
                    answer15Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer25Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer35Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer45Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                    answer55Answers.GetComponentInChildren<TMP_Text>().color = typingColor;
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    // Method that give a visual feedback on the correct and incorrect answers and correct or incorrect given answers
    //-------------------------------------------------------------------------------------------------------------------------------------------

    // Method that displays the correct or incorrect answer when clicking on the selection confirmation button after solving an input question
    public void DisplayRestultInput()
    {
        // Get the current question path
        string questionPath = Questions.questionArray[Questions.currentQuestionIndex];

        // Extract the question object
        string json = File.ReadAllText(questionPath);
        InputQuestion question = JsonUtility.FromJson<InputQuestion>(json);

        // Initialize the boolean that tells if the given answer was correct or not
        bool answeredCorrectly = true;

        // Check if the answer is correct
        if(answerFieldInput.text == question.answer)
        {
            // Case it is the correct answer, change the color of the text of the input field to green
            GameObject.Find("TextAnswerInput").GetComponent<TMP_Text>().color = correctColor;

            // Activate the close button and deactivate the confirm button
            closeQuestionInput.gameObject.SetActive(true);
            confirmAnswerInput.gameObject.SetActive(false);

            // Deactivate the input field (make it uninteractable)
            answerFieldInput.interactable = false;
            

        } else {

            // Check if it was a spelling mistake (capital letter missing or too much at the begining)
            bool sameExceptCapitalLetter = CheckIfBothOrNoneStartWithCapitalLetter(answerFieldInput.text, question.answer);

            if(sameExceptCapitalLetter == true)
            {
                // Case it is the correct answer except for the first latter that had/missed capitalization, change the color of the text of the input field to green
                GameObject.Find("TextAnswerInput").GetComponent<TMP_Text>().color = correctColor;

                // Change the text so that it has the capitalization of the answer
                answerFieldInput.text = question.answer;

                // Activate the close button and deactivate the confirm button
                closeQuestionInput.gameObject.SetActive(true);
                confirmAnswerInput.gameObject.SetActive(false);

                // Deactivate the input field (make it uninteractable)
                answerFieldInput.interactable = false;

            } else {
                // Case both words are different, open the correct answer preview
                previewCorrectAnswerInput.SetActive(true);

                // Disable the text input field
                answerFieldInput.gameObject.SetActive(false);

                // Set the text objects on the right string
                yourWrongAnswer.text = answerFieldInput.text;
                theCorrectAnswer.text = question.answer;

                // Activate the close button and deactivate the confirm button
                closeQuestionInput.gameObject.SetActive(true);
                confirmAnswerInput.gameObject.SetActive(false);

                // Set the boolean that tells that the answer was correct or not to false
                answeredCorrectly = false;
            }
        }

        // Check if the question was answered correctly or not and give visual feedback
        DisplayFeedbackButton(feedbackInput, answeredCorrectly);
    }

    // Method that checks if both strings begin with or without capital letter, if not check if it is the same letter
    public bool CheckIfBothOrNoneStartWithCapitalLetter(string firstWord, string secondWord)
    {
        // Check if the first characters are digits or letters
        if(Char.IsDigit(firstWord, 0) == false && Char.IsDigit(secondWord, 0) == false)
        {
            // Case the first characters are letters
            // Check if the first character of both words is a capital letter
            bool firstCapital = Char.IsUpper(firstWord, 0);
            bool secondCapital = Char.IsUpper(secondWord, 0);

            // Compare the boolean
            if(firstCapital == secondCapital)
            {
                // Case both are small or both are capital, both words are different
                return false;

            } else {

                // One is writen with a capital, the other is written small, check if it is the same word
                if(firstCapital == false)
                {
                    // Case it is the first word that is written small, add the capital letter and check the equality
                    string capitalizedFirstWord = WriteFirstLetterCapital(firstWord);

                    // Check if the capitalized word is identical to the other
                    if(capitalizedFirstWord == secondWord)
                    {
                        // Case both words are identical except for first character capitalization
                        return true;

                    } else {

                        // Case both words are different, return false
                        return false;
                    }

                } else {

                    // Case it is the second word that is written small, add the capital letter and check the equality
                    string capitalizedSecondWord = WriteFirstLetterCapital(secondWord);

                    // Check if the capitalized word is identical to the other
                    if(capitalizedSecondWord == firstWord)
                    {
                        // Case both words are identical except for first character capitalization
                        return true;

                    } else {

                        // Case both words are different, return false
                        return false;
                    }
                }
            }

        } else {

            // Case the first character is a digit, both words are different
            return false;
        }
    }

    // Method that takes a word, and returns it with a capital letter as first letter
    public string WriteFirstLetterCapital(string word)
    {
        // Extract the first letter
        string firstLetter = word.Substring(0, 1);

        // Make it a capital letter
        firstLetter = firstLetter.ToUpper();

        // Get the end of the word
        string ending = word.Substring(1);

        // Return the capital letter with the word ending
        return firstLetter + ending;

    }

    // Method that displays the correct or incorrect answer when clicking on the selection confirmation button after solving an multiple choice question
    public void DisplayRestultMC()
    {
        // Get the current question path
        string questionPath = Questions.questionArray[Questions.currentQuestionIndex];

        // Extract the question object
        string json = File.ReadAllText(questionPath);
        MultipleChoiceQuestion question = JsonUtility.FromJson<MultipleChoiceQuestion>(json);

        // Initialize the boolean variable that gives the information if all correct answers were selected and no incorrect answer was selected
        bool answeredCorrectly = true;

        // Go over all possible answers, and check if they were selected or not
        for(int index = 0; index < question.numberOfAnswers; index = index + 1)
        {
            // Get the button that correspond to the index
            Button currentButton = GetQuestionButtonFromIndex(index, question.numberOfAnswers);

            Debug.Log(AnswerShouldBeCorrect(index, question));

            // Check if the answer should be correct and was selected
            if(AnswerShouldBeCorrect(index, question) == true)
            {
                // Check if the user selected the correct answer
                if(currentButton.GetComponent<Image>().color == selectedColor)
                {
                    // User did select this correct answer as correct
                    currentButton.GetComponentInChildren<TMP_Text>().color = correctColor;

                } else {

                    // User did not select this correct answer as incorrect
                    currentButton.GetComponentInChildren<TMP_Text>().color = incorrectColor;

                    // Did not select correct answer, question answered incorrectly
                    answeredCorrectly = false;
                }

                // Answer was correct, give the button a greenish tint
                currentButton.GetComponent<Image>().color = correctButtonColor;
                
            } else {

                if(currentButton.GetComponent<Image>().color == selectedColor)
                {
                    // User did select this incorrect answer as correct
                    currentButton.GetComponentInChildren<TMP_Text>().color = correctColor;

                    // Did select incorrect answer, question answered incorrectly
                    answeredCorrectly = false;

                } else {

                    // User did not select this incorrect answer as incorrect
                    currentButton.GetComponentInChildren<TMP_Text>().color = incorrectColor;
                }

                // Answer was incorrect, give the button a redish tint
                currentButton.GetComponent<Image>().color = incorrectButtonColor;
            }

            // Make the button not interactable
            currentButton.interactable = false;
        }

        // Check if the question was answered correctly or not and give visual feedback
        DisplayFeedbackButton(feedbackMC, answeredCorrectly);

        // Activate the close button and deactivate the confirm button
        closeQuestionMC.gameObject.SetActive(true);
        confirmAnswerMC.gameObject.SetActive(false);
    }

    // Method that displays the correctly or incorrectly answered question
    public void DisplayFeedbackButton(Button button, bool answer)
    {
        // Enable the button
        button.gameObject.SetActive(true);

        // Check if the question was answered correctly or not
        if(answer == true)
        {
            // Set the answer correct text
            button.GetComponentInChildren<TMP_Text>().text = "Your answer was correct!";

            // Set the correct color
            button.GetComponentInChildren<TMP_Text>().color = correctColor;

        } else {

            // Set the answer incorrect text
            button.GetComponentInChildren<TMP_Text>().text = "Your answer was incorrect.";

            // Set the incorrect color
            button.GetComponentInChildren<TMP_Text>().color = incorrectColor;
        }
    }

    // Method that returns if the answer with given index should be true or not
    public bool AnswerShouldBeCorrect(int index, MultipleChoiceQuestion question)
    {
        // Return the right boolean value depending on the index
        switch(index)
        {
            case 0:
                return question.answer1Correct;
            break;
            case 1:
                return question.answer2Correct;
            break;
            case 2:
                return question.answer3Correct;
            break;
            case 3:
                return question.answer4Correct;
            break;
            case 4:
                return question.answer5Correct;
            break;
        }

        // This case will never happen
        return question.answer5Correct;
    }

    // Method that returns you the right button given the index
    public Button GetQuestionButtonFromIndex(int index, int numberOfAnswers)
    {
        // Check what is the number of answers
        if(numberOfAnswers == 2)
        {
            // Case there are two answers, get the button of the two answers multiple choice preview
             switch(index)
            {
                case 0:
                    return answer12Answers;
                break;
                case 1:
                    return answer22Answers;
                break;
            }

        } else if(numberOfAnswers == 3)
        {
            // Case there are three answers, get the button of the three answers multiple choice preview
            switch(index)
            {
                case 0:
                    return answer13Answers;
                break;
                case 1:
                    return answer23Answers;
                break;
                case 2:
                    return answer33Answers;
                break;
            }

        } else if(numberOfAnswers == 4)
        {
            // Case there are four answers, get the button of the four answers multiple choice preview
            switch(index)
            {
                case 0:
                    return answer14Answers;
                break;
                case 1:
                    return answer24Answers;
                break;
                case 2:
                    return answer34Answers;
                break;
                case 3:
                    return answer44Answers;
                break;
            }
            
        }else {

            // Case there are five answers, get the button of the five answers multiple choice preview
            switch(index)
            {
                case 0:
                    return answer15Answers;
                break;
                case 1:
                    return answer25Answers;
                break;
                case 2:
                    return answer35Answers;
                break;
                case 3:
                    return answer45Answers;
                break;
                case 4:
                    return answer55Answers;
                break;
            }
        }

        // This case will never happen
        return answer55Answers;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    // Method that create the question array, shuffeling the questions in the level directory, and that changes question after the current question was answered
    //-------------------------------------------------------------------------------------------------------------------------------------------
    
    // The path to the current level directory
    public string levelDirectoryPath;

    public Button startButton;

    private IEnumerator GetJsonFiles(string additionalPath)
    {
        string path = Application.streamingAssetsPath + additionalPath;
        UnityWebRequest uwr = new UnityWebRequest(path);
        
        yield return uwr.SendWebRequest();
    
        string newPath = Application.persistentDataPath + additionalPath;
        File.WriteAllBytes(newPath, uwr.downloadHandler.data);
    }

    
    public void ReadJson(string additionalPath)
    {
        string path = Application.persistentDataPath + additionalPath;
        StreamReader json = new StreamReader(path);
        string input = json.ReadToEnd();
        Log description = JsonUtility.FromJson<Log>(input);

        // Write the number of questions on the button
        startButton.GetComponentInChildren<TMP_Text>().text = description.numberOfQuestions.ToString();
    }


    // Method that creates the question array (not shuffled)
    public void InitializeQuestionArray()
    {
        GetJsonFiles("/Level2/Description.json");

        ReadJson("/Level2/Description.json");
        // // Get the path to the description file
        // string pathToDescription = Path.Combine(Questions.pathToLevel, "Description.json");

        // // Initialize the string 
        // string myText = "";

        // // Get the data
        // UnityWebRequest data = new UnityWebRequest.Get(pathToDescription);

        // // If the string is not empty, get the string
        // if(string.IsNullOrEmpty(data.error))
        // {
        //     myText = data.text;
        // }

        // // Extract the description object
        // Log description = JsonUtility.FromJson<Log>(myText);
        
        // // Write the number of questions on the button
        // startButton.GetComponentInChildren<TMP_Text>().text = description.numberOfQuestions.ToString();

        //

        // // Get the array of question files
        // string[] questions = Directory.GetFiles(Questions.pathToLevel, "Question*", SearchOption.TopDirectoryOnly);
        
        // // Set the Questions.questionArray right
        // Questions.questionArray = questions;

        // // Set the last question index
        // Questions.lastQuestionIndex = Questions.questionArray.Length - 1;

        // // Set the current question index to 0
        // Questions.currentQuestionIndex = 0;
    }

    // Initialize random number generator
    private readonly System.Random random = new System.Random();

    // Method that shuffles an array
    public string[] ShuffleQuestionArray(string[] array)
    {
        // Get the length of the question array
        int length = array.Length;

        // Initialize the swap index
        int swapIndex = 0;

        // Initialize the loop index
        int index = length - 1;

        // Shuffle the question array
        while(index >= 0)
        {
            // Get a random number
            swapIndex = random.Next(0, index);

            // Copy entry at swapIndex to the entry index of the array
            string value = array[swapIndex];
            array[swapIndex] = array[index];
            array[index] = value;

            // Reduce the index by one
            index = index - 1;
        }

        // Return the shuffled array
        return array;
    }

    // Method that initializes everything when a level is started
    public void InitializeRound()
    {
        // // // Get the path
        // // //string filePath = Path.Combine(Application.persistentDataPath, "description.json");

        // string path1 = "sdcard/AppAnna/Description.json";
        // string path2 = "/mnt/sdcard/AppAnna/Description.json";
        // string path3 = "/sdcard/AppAnna/Description.json";
        // string path4 = "/storage/sdcard0/AppAnna/Description.json";
        // string path5 = "/storage/sdcard1/AppAnna/Description.json";
        // string path6 = "/sdcard/external_sd/AppAnna/Description.json";
        // string path7 = "/mnt/extSdCard/AppAnna/Description.json";
        // string path8 = "/storage/extSdCard/AppAnna/Description.json";

        // int int1 = 1;
        // int int2 = 2;
        // int int3 = 3;
        // int int4 = 4;
        // int int5 = 5;
        // int int6 = 6;
        // int int7 = 7;
        // int int8 = 8;

        // string workingPaths = "";

        // if(File.Exists(path1))
        // {
        //     workingPaths = workingPaths + int1.ToString();
        // }
        // if(File.Exists(path2))
        // {
        //     workingPaths = workingPaths + int2.ToString();
        // }
        // if(File.Exists(path3))
        // {
        //     workingPaths = workingPaths + int3.ToString();
        // }
        // if(File.Exists(path4))
        // {
        //     workingPaths = workingPaths + int4.ToString();
        // }
        // if(File.Exists(path5))
        // {
        //     workingPaths = workingPaths + int5.ToString()   ;
        // }
        // if(File.Exists(path6))
        // {
        //     workingPaths = workingPaths + int6.ToString()   ;
        // }
        // if(File.Exists(path7))
        // {
        //     workingPaths = workingPaths + int7.ToString()   ;
        // }
        // if(File.Exists(path8))
        // {
        //     workingPaths = workingPaths + int8.ToString()   ;
        // }

        // // Rename the button
        // startButton.GetComponentInChildren<TMP_Text>().text = workingPaths;

        // -----------------------------------------

        string filePath = Path.Combine(Application.persistentDataPath, "Level1");

        startButton.GetComponentInChildren<TMP_Text>().text = filePath;

        // Rename the button
        // startButton.GetComponentInChildren<TMP_Text>().text = workingPaths;

        // Check if the directory exists
        if(Directory.Exists(filePath) == false)
        {
            // Create the directory
            Directory.CreateDirectory(filePath);
        }

        // If the folder for the question saving is empty, then copy the files from path4 in it
        string[] fileArray = GetFilesArray(filePath); 
        if(fileArray == null || fileArray[0] == "")
        {
            // Case empty, transfer the data
            string[] levelFiles = GetFilesArray("/storage/sdcard0/AppAnna/");
            
            if(levelFiles == null || levelFiles[0] == "")
            {
                startButton.GetComponentInChildren<TMP_Text>().text = "level files are empty";
            }
            foreach(string file in levelFiles)
            {
                startButton.GetComponentInChildren<TMP_Text>().text = "copying data";
                // Read the data
                string loadData = File.ReadAllText(file);

                // Get the fileName
                string name = Path.GetFileName(file);
                Debug.Log("Current File name: " + name);

                // Save it
                File.WriteAllText(Path.Combine(filePath, name), loadData);
            }
        } else {
            startButton.GetComponentInChildren<TMP_Text>().text = "save folder is not empty";
        }

        // -----------------------------------------

        // // Get all files in the folder and coppy them to the persistent data path
        // string[] files = GetFilesArray(levelDirectoryPath);
        // foreach(string file in files)
        // {
        //     // Read the data
        //     string loadData = File.ReadAllText(file);

        //     // Get the fileName
        //     string name = Path.GetFileName(file);
        //     Debug.Log("Current File name: " + name);

        //     // Save it
        //     // File.WriteAllText(Path.Combine(Application.persistentDataPath, name), loadData);
        // }

        // // // // Create the log object
        // // // Log description = new Log();

        // // // // Fill it
        // // // description.description = "This is the description";
        // // // description.heading = "My name";

        // // // // Convert it to json
        // // // string jsonData = JsonUtility.ToJson(description);

        // // // // Save it
        // // // File.WriteAllText(filePath, jsonData);

        // Check if it works and can be loaded
        // TestFunction();

        // // Initialize the question array
        // InitializeQuestionArray();

        // // Shuffle the question array
        // Questions.questionArray = ShuffleQuestionArray(Questions.questionArray);

        // // Load all models
        // ImportAllModels();

        // // Wait for all models to be loaded and enable the question menu and display the models
        // // StartCoroutine(WaitForAllModelsToBeLoaded());

        // Debug.Log("The current question is: " + Questions.questionArray[Questions.currentQuestionIndex]);
    }

    // Method that returns the array of files in a directory
    static string[] GetFilesArray(string path) 
    {
        string[] files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        return files;
    }

    // Test function to test the saving of files on the android device
    public void TestFunctionCopyFiles()
    {
        // Case empty, transfer the data
        string[] levelFiles = GetFilesArray(levelDirectoryPath);
        if(levelFiles == null || levelFiles[0] == "")
        {
            startButton.GetComponentInChildren<TMP_Text>().text = "level files are empty";
        }
        foreach(string file in levelFiles)
        {
            startButton.GetComponentInChildren<TMP_Text>().text = "copying data";
            // Read the data
            string loadData = File.ReadAllText(file);

            // Get the fileName
            string name = Path.GetFileName(file);
            Debug.Log("Current File name: " + name);

            // Save it
            File.WriteAllText(Path.Combine(Path.Combine(levelDirectoryPath,"Test"), name), loadData);
        }
    }

    // Test function to test the saving of files on the android device
    public void TestFunction()
    {
        // Get the path
        string filePath = Path.Combine(Application.persistentDataPath, "Description.json");

        // Read the data
        string loadData = File.ReadAllText(filePath);

        // Get the gameobject
        Log description = JsonUtility.FromJson<Log>(loadData);

        // Print the description on the button
        startButton.GetComponentInChildren<TMP_Text>().text = description.description;

    }

    // // Method that waits that all models have been imported, before displaying models
    // IEnumerator WaitForAllModelsToBeLoaded()
    // {
    //     Debug.Log ("Waiting for all models to be loaded....");

    //     // Wait for the last model to be loaded
    //     yield return new WaitUntil(() => Questions.numberOfModelsLoaded >= Questions.numberOfModels);

    //     Debug.Log ("The view model menu will be enabled");

    //     ActivateViewModels();
    // }

    // Method that activates the view model menu and enables the user to open the first question
    public void ActivateViewModels()
    {
        // After waiting, enable the view model menu
        viewModel.SetActive(true);

        // Display the models of the current questions
        DisplayModels();

        // Disable the start menu
        startMenu.SetActive(false);
    }

    // Method that closes the current question, and changes the current question index
    public void GoToNextQuestion()
    {
        // Check if the the current question index reached the end of the index
        if(Questions.currentQuestionIndex < Questions.lastQuestionIndex)
        {
            // Case the current question index is smaller than the last question index, increase the current question index by one
            Questions.currentQuestionIndex = Questions.currentQuestionIndex + 1;

        } else {

            // Case the current question index reached the end of the array, shuffle the array and set the current question index to 0
            Questions.questionArray = ShuffleQuestionArray(Questions.questionArray);
            Questions.currentQuestionIndex = 0;
        }

        // Disable the feedback buttons
        feedbackInput.gameObject.SetActive(false);
        feedbackMC.gameObject.SetActive(false);

        // // Display the models of the current questions
        // StartCoroutine(WaitForAllModelsToBeLoaded());

        // // After waiting, enable the view model menu
        // viewModel.SetActive(true);

        // Display the models of the current questions
        DisplayModels();

        Debug.Log("The current question is: " + Questions.questionArray[Questions.currentQuestionIndex]);
    }

    // Define the game object under which all models are saved (so that they can be found with their name, even if they have the same name as a game object)
    public GameObject saveModelObject;

    // Method that imports all models, and sets them invisible. Is done at the begining of a round so that no wait time is needed while playing.
    public async void ImportAllModels()
    {
        // Initialize the object importer
        ObjImporter objImporter = new ObjImporter();
        ServiceManager.RegisterService(objImporter);

        // Get the array of models
        string[] models = Directory.GetFiles(Questions.pathToLevel, "Model*", SearchOption.TopDirectoryOnly);

        // Set the number of models
        Questions.numberOfModels = models.Length;

        // Import all models
        foreach(string model in models)
        {
            // Access the model gameobject
            string json = File.ReadAllText(model);

            // Extract the gameobject
            Model modelObject = JsonUtility.FromJson<Model>(json);

            // Import the first model
            string url = urlBegin + modelObject.modelName;
            GameObject obj = await ServiceManager.GetService<ObjImporter>().ImportAsync(url);

            // Initialize the model (resize it correctly) and set it as the child of the same model object
            InitializeModel(obj, saveModelObject);

            // When the model finished loading, increase the loaded model counter
            Questions.numberOfModelsLoaded = Questions.numberOfModelsLoaded + 1;

            Debug.Log("The current number of models that are loaded is: " + Questions.numberOfModelsLoaded);
            Debug.Log("Currently we have: " + Questions.numberOfModelsLoaded + " >= "  + Questions.numberOfModels);
        }
        Debug.Log("The number of models is: " + Questions.numberOfModels);

        ActivateViewModels();
    }
}