using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class Play : MonoBehaviour {

    public GameObject ui;
    public UnityEngine.UI.Text questionText;
    public UnityEngine.UI.Image answersContainer;
    public UnityEngine.UI.Text answer1Text;
    public UnityEngine.UI.Text answer2Text;
    public UnityEngine.UI.Text answer3Text;

    public List<DialogNode> nodes = new List<DialogNode>();
    public DialogNode currentNode;

    public TextAsset dialogs;

    public List<Elector> electors = new List<Elector>();

    public GameObject electorsPanel;
    public GameObject electorPanelPrefab;
    public List<ElectorPanel> electorsPanels = new List<ElectorPanel>();

    public GameObject popPanelPrefab;

    public UnityEngine.UI.Text scoreObj;
    public UnityEngine.UI.Text commentObj;

    public GameObject firstHide;
    public GameObject creditsScreen;
    public GameObject bigTitleScreen;
    public GameObject gameOverFirstScreen;
    public UnityEngine.UI.Text scoreCommentTextObj;
    public UnityEngine.UI.Text scoreDescriptionTextObj;
    public GameObject gameOverFinalScreen;
    public GameObject gameOverHideScreen;

    public Sprite[] idleSprites;
    public Sprite[] talkSprites;
    public Sprite[] idleSprites2;
    public Sprite[] talkSprites2;

    public GameObject character;
    public SpriteAnimation characterAnimation;

    public GameObject character2;
    public SpriteAnimation characterAnimation2;

    private bool waitingForInput = false;

    private bool updatingText = false;
    private bool pressed1 = false;
    private bool pressed2 = false;
    private bool pressed3 = false;
    private bool hasClicked1 = false;
    private bool hasClicked2 = false;
    private bool hasClicked3 = false;

    private bool gameStarted = false;

    //Audio
    public GameObject soundSourcePrefab;
    public AudioClip defaultSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip musicSound;
    public AudioClip creditsChargeSound;
    public AudioClip creditsActivateSound;
    public AudioClip creditsDeploySound;

    public SoundSource music;

    // Use this for initialization
    void Start () {
        this.dialogs = Resources.Load<TextAsset>("scenario");

        this.createConversation(this.dialogs.text);
        this.currentNode = this.nodes[0];

        List<Elector> anars = new List<Elector>();
        List<Elector> gauches = new List<Elector>();
        List<Elector> droites = new List<Elector>();
        List<Elector> extremes = new List<Elector>();

        for (int i = 0; i < 100; i++)
        {
            Elector newElector = new Elector();

            switch (newElector.main)
            {
                case Elector.ElectorCategory.Anar:
                    anars.Add(newElector);
                    break;
                case Elector.ElectorCategory.Gauche:
                    gauches.Add(newElector);
                    break;
                case Elector.ElectorCategory.Droite:
                    droites.Add(newElector);
                    break;
                case Elector.ElectorCategory.Extreme:
                    extremes.Add(newElector);
                    break;
            }
        }

        //sort electors
        this.electors = new List<Elector>();
        this.electors.AddRange(anars);
        this.electors.AddRange(gauches);
        this.electors.AddRange(droites);
        this.electors.AddRange(extremes);

        foreach (Elector elector in this.electors)
        {
            GameObject panelObj = (GameObject)Instantiate(this.electorPanelPrefab);
            panelObj.transform.SetParent(this.electorsPanel.transform);

            ElectorPanel panel = panelObj.GetComponent<ElectorPanel>();
            panel.elector = elector;
            this.electorsPanels.Add(panel);
        }

        //StartCoroutine(this.gameOverRoutine());
        StartCoroutine(this.handleCharacterAnimation());
        StartCoroutine(this.handleCharacterAnimation2());

        StartCoroutine(this.startGame());
    }

    private IEnumerator startGame()
    {
        //this.creditsChargeSound = null;

        //this.creditsChargeSound = (AudioClip)Resources.Load("minecharge");

        while(this.musicSound == null || this.creditsChargeSound == null || this.creditsDeploySound == null)
        {
            yield return new WaitForEndOfFrame();
        }

        this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
        this.gameOverHideScreen.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        this.playSound(6);

        this.updatingText = true;

        this.playSound(4);

        while(this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha > 0.0f)
        {
            this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha -= 0.33f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        this.playSound(5);

        this.firstHide.SetActive(false);

        yield return new WaitForSeconds(2.0f);

        while (this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha < 1.0f)
        {
            this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha += 1.0f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        this.creditsScreen.SetActive(false);

        this.bigTitleScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
        this.playSound(1);
        this.playSound(3);

        while (this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha > 0.0f)
        {
            this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha -= 2.0f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3.0f);

        while (this.bigTitleScreen.GetComponent<CanvasGroup>().alpha > 0.0f)
        {
            this.bigTitleScreen.GetComponent<CanvasGroup>().alpha -= 1.0f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        this.bigTitleScreen.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        foreach(ElectorPanel panel in this.electorsPanels)
        {
            panel.deactivated = false;
            this.playSound(0);
            yield return new WaitForSeconds(0.0625f);
        }

        yield return new WaitForSeconds(2.0f);

        this.updateConversation(this.currentNode);

        this.updatingText = false;
        this.gameStarted = true;
    }
	
	// Update is called once per frame
	void Update () {
        this.pressed1 = false;
        this.pressed2 = false;
        this.pressed3 = false;

        if(this.hasClicked1 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.hasClicked1 = false;
            this.pressed1 = true;
        }
        if (this.hasClicked2 || Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.hasClicked2 = false;
            this.pressed2 = true;
        }
        if (this.hasClicked3 || Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.hasClicked3 = false;
            this.pressed3 = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(!this.updatingText && (this.pressed1 || this.pressed2 || this.pressed3)){
            string selectedAnswer = "";

            if(this.pressed1){
                selectedAnswer = this.currentNode.answers[0];
            }
            else if(this.pressed2){
                if(this.currentNode.answers.Count > 1)
                    selectedAnswer = this.currentNode.answers[1];
                else
                    selectedAnswer = this.currentNode.answers[0];
            }
            else if(this.pressed3){
                if(this.currentNode.answers.Count > 2)
                    selectedAnswer = this.currentNode.answers[2];
                else if(this.currentNode.answers.Count > 1)
                    selectedAnswer = this.currentNode.answers[1];
                else
                    selectedAnswer = this.currentNode.answers[0];
            }

            GameObject eventSystem = GameObject.Find("EventSystem");
            eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

            this.updateConversation(selectedAnswer);

            if (
                !this.pressed1 || (this.pressed1 && this.currentNode.answers[0] != "2" && this.currentNode.answers[0] != "4")
            )
            {
                this.characterAnimation = new SpriteAnimation(this.character, this.talkSprites2, 0, UnityEngine.Random.Range(100, 200));
                StartCoroutine(this.characterAnimation.update());
            }
        }

        if(this.waitingForInput)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                this.waitingForInput = false;
            }
        }

        if(this.questionText.text == "")
        {
            this.questionText.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(
                    this.questionText.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color.r,
                    this.questionText.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color.g,
                    this.questionText.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color.b,
                    0.0f
            );
        }
        else
        {
            this.questionText.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(
                    this.questionText.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color.r,
                    this.questionText.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color.g,
                    this.questionText.transform.parent.gameObject.GetComponent<UnityEngine.UI.Image>().color.b,
                    0.9f
            );
        }

        if (this.answer1Text.text == "" && this.answer2Text.text == "" && this.answer3Text.text == "")
        {
            this.answersContainer.color = new Color(
                    this.answersContainer.color.r,
                    this.answersContainer.color.g,
                    this.answersContainer.color.b,
                    0.0f
            );
        }
        else
        {
            this.answersContainer.color = new Color(
                    this.answersContainer.color.r,
                    this.answersContainer.color.g,
                    this.answersContainer.color.b,
                    0.9f
            );
        }

        if (this.answer1Text.text == "")
            this.answer1Text.transform.parent.gameObject.SetActive(false);
        else
            this.answer1Text.transform.parent.gameObject.SetActive(true);
        
        if (this.answer2Text.text == "")
            this.answer2Text.transform.parent.gameObject.SetActive(false);
        else
            this.answer2Text.transform.parent.gameObject.SetActive(true);
        
        if (this.answer3Text.text == "")
            this.answer3Text.transform.parent.gameObject.SetActive(false);
        else
            this.answer3Text.transform.parent.gameObject.SetActive(true);

        this.answer1Text.text = this.answer1Text.text.TrimEnd();
        this.answer2Text.text = this.answer2Text.text.TrimEnd();
        this.answer3Text.text = this.answer3Text.text.TrimEnd();
    }

    public void clickChoice1(){
        this.hasClicked1 = true;
    }

    public void clickChoice2(){
        this.hasClicked2 = true;
    }

    public void clickChoice3(){
        this.hasClicked3 = true;
    }

    public void updateConversation(DialogNode node)
    {
        this.answer1Text.text = "";
        this.answer2Text.text = "";
        this.answer3Text.text = "";

        for (int i = 0; i < node.answers.Count; i++)
        {
            string nextText = (i + 1) + ". " + this.getNodeById(node.answers[i]).content.TrimEnd() + "\n\n";
            UnityEngine.UI.Text selectedText = null;
            
            if(i == 0){
                selectedText = this.answer1Text;
            }
            else if(i == 1){
                selectedText = this.answer2Text;
            }
            else if(i == 2){
                selectedText = this.answer3Text;
            }

            selectedText.text = nextText;
        }

        this.questionText.text = node.content;
    }

    public void updateConversation(string nodeId)
    {
        DialogNode selectedByPlayerNode = this.getNodeById(nodeId);
        selectedByPlayerNode.alreadyVisited = true;
        //Debug.Log("Player selected node #"+selectedByPlayerNode.id);

        DialogNode node = null;//current node will be direct answer to selected answer
        foreach (string possibleDirectAnswerId in selectedByPlayerNode.answers)
        {
            DialogNode possibleDirectAnswer = this.getNodeById(possibleDirectAnswerId);
            if(!possibleDirectAnswer.alreadyVisited)
            {
                node = possibleDirectAnswer;
                //Debug.Log("Direct answer is node #" + node.id);
                break;
            }
        }
        if (node != null){
            this.currentNode = node;
            this.currentNode.alreadyVisited = true;

            string[] playerAnswers = new string[3];
            for (int i = 0; i < this.currentNode.answers.Count; i++)
            {
                DialogNode playerAnswerNode = this.getNodeById(this.currentNode.answers[i]);
                //Debug.Log("#" + playerAnswerNode.id);
                if (!playerAnswerNode.alreadyVisited){
                    playerAnswers[i] = ((i + 1) + ". " + playerAnswerNode.content).TrimEnd();
                    //Debug.Log("is a possible player answer");
                }
            }

            StartCoroutine(this.updateTextRoutine(selectedByPlayerNode, node.content, playerAnswers));
        }
        else
        {
            //Debug.Log("#"+nodeId+": couldn't find this node");
            //Is end TODO use =>END in scenario file
            StartCoroutine(this.gameOverRoutine());
        }
    }

    private IEnumerator updateTextRoutine(DialogNode chosenAnswer, string questionString, string[] playerAnswers)
    {
        /*//Debug.Log("Chosen:");
        //Debug.Log(chosenAnswer.id);
        //Debug.Log("Children:");
        foreach(string answer in chosenAnswer.answers){
            //Debug.Log(answer);
        }*/

        this.updatingText = true;

        int numberOfElectorsConvinced = 0;
        foreach(Elector elector in this.electors)
        {
            if(elector.sympathy >= 0.5f)
            {
                numberOfElectorsConvinced++;
            }
        }

        chosenAnswer.applyEffect();

        int newNumberOfElectorsConvinced = 0;
        foreach (Elector elector in this.electors)
        {
            if (elector.sympathy >= 0.5f)
            {
                newNumberOfElectorsConvinced++;
            }
        }

        if (chosenAnswer.effect != "") {
            GameObject popObj = (GameObject)Instantiate(this.popPanelPrefab);
            popObj.transform.SetParent(this.ui.transform);
            PopPanel pop = popObj.GetComponent<PopPanel>();
            if ((newNumberOfElectorsConvinced - numberOfElectorsConvinced) >= 0)
            {
                pop.contentText.text = "+" + (newNumberOfElectorsConvinced - numberOfElectorsConvinced) + " électeurs";
            }
            else
            {
                pop.contentText.text = "-" + Mathf.Abs(newNumberOfElectorsConvinced - numberOfElectorsConvinced) + " électeurs";
            }

            if ((newNumberOfElectorsConvinced - numberOfElectorsConvinced) > 0){
                this.playSound(1);
            }
            else if ((newNumberOfElectorsConvinced - numberOfElectorsConvinced) == 0)
            {
                this.playSound(0);
            }
            else
            {
                this.playSound(2);
            }

            this.scoreObj.text = newNumberOfElectorsConvinced + " %";
            if(newNumberOfElectorsConvinced > 50)
            {
                this.commentObj.text = "Majorite absolue";
                StartCoroutine(this.blinkComment());
            }
            else if (newNumberOfElectorsConvinced >= 25)
            {
                this.commentObj.text = "Majorite relative";
            }
            else if (newNumberOfElectorsConvinced >= 10)
            {
                this.commentObj.text = "Candidat minoritaire";
            }
            else
            {
                this.commentObj.text = "Candidature derisoire";
            }
        }

        this.answer1Text.text = "";
        this.answer2Text.text = "";
        this.answer3Text.text = "";

        yield return new WaitForSeconds(2.0f);

        this.characterAnimation2 = new SpriteAnimation(this.character2, this.talkSprites, 0, UnityEngine.Random.Range(100, 200));
        StartCoroutine(this.characterAnimation2.update());
        this.appearText(this.questionText, questionString);

        bool continued = true;
        while(continued)
        {
            if(this.questionText.text.Length >= questionString.Length)
            {
                continued = false;
            }

            if(this.gameStarted && (this.pressed1 || this.pressed2 || this.pressed3))
            {
                this.questionText.text = questionString;
                continued = false;
            }
            yield return new WaitForEndOfFrame();
        }

        this.answer1Text.text = playerAnswers[0];
        this.answer2Text.text = playerAnswers[1];
        this.answer3Text.text = playerAnswers[2];

        this.updatingText = false;
    }

    private IEnumerator blinkComment()
    {
        for(int i = 0; i < 5; i++)
        {
            if(this.commentObj.GetComponent<CanvasGroup>().alpha == 1.0f)
            {
                this.commentObj.GetComponent<CanvasGroup>().alpha = 0.0f;
            }
            else
            {
                this.commentObj.GetComponent<CanvasGroup>().alpha = 1.0f;
            }
            yield return new WaitForSeconds(0.1f);
        }

        this.commentObj.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    private IEnumerator gameOverRoutine()
    {
        int scoreAnar = 0;
        int scoreGauche = 0;
        int scoreDroite = 0;
        int scoreExtreme = 0;

        foreach(Elector elector in this.electors)
        {
            switch (elector.main)
            {
                case Elector.ElectorCategory.Anar:
                    if(elector.sympathy >= 0.5f)
                    {
                        scoreAnar++;
                    }
                    break;
                case Elector.ElectorCategory.Gauche:
                    if (elector.sympathy >= 0.5f)
                    {
                        scoreGauche++;
                    }
                    break;
                case Elector.ElectorCategory.Droite:
                    if (elector.sympathy >= 0.5f)
                    {
                        scoreDroite++;
                    }
                    break;
                case Elector.ElectorCategory.Extreme:
                    if (elector.sympathy >= 0.5f)
                    {
                        scoreExtreme++;
                    }
                    break;
            }
        }

        int numberOfElectors = (scoreAnar + scoreGauche + scoreDroite + scoreExtreme);
        this.scoreCommentTextObj.text = "Vous avez convaincu "+numberOfElectors+" électeurs de voter pour vous.\r\n\r\n";
        if(numberOfElectors > 50){
            this.scoreCommentTextObj.text += "Vous êtes certain d'être élu aux prochaines élections présidentielles.\r\n\r\n";
        }
        else if (numberOfElectors >= 25)
        {
            this.scoreCommentTextObj.text += "Vous êtes certains de passer le premier tour des prochaines élections présidentielles.\r\n\r\n";
        }
        else
        {
            this.scoreCommentTextObj.text += "Vous êtes certain de ne pas passer le premier tour des prochaines élections présidentielles.\r\n\r\n";
        }
        this.scoreCommentTextObj.text += "Parmi ces électeurs se trouvent :";

        this.scoreDescriptionTextObj.text = "";

        int scoreAnarLength = scoreAnar.ToString().Length;
        //Debug.Log(scoreAnarLength);
        for(int i = 0; i < 57 - scoreAnarLength; i++){
            this.scoreDescriptionTextObj.text += ".";
        }
        this.scoreDescriptionTextObj.text += ""+scoreAnar;
        this.scoreDescriptionTextObj.text += "\r\n";

        int scoreGaucheLength = scoreGauche.ToString().Length;
        for (int i = 0; i < 57 - scoreGaucheLength; i++)
        {
            this.scoreDescriptionTextObj.text += ".";
        }
        this.scoreDescriptionTextObj.text += "" + scoreGauche;
        this.scoreDescriptionTextObj.text += "\r\n";

        int scoreDroiteLength = scoreDroite.ToString().Length;
        for (int i = 0; i < 57 - scoreDroiteLength; i++)
        {
            this.scoreDescriptionTextObj.text += ".";
        }
        this.scoreDescriptionTextObj.text += "" + scoreDroite;
        this.scoreDescriptionTextObj.text += "\r\n";

        int scoreExtremeLength = scoreExtreme.ToString().Length;
        for (int i = 0; i < 57 - scoreExtremeLength; i++)
        {
            this.scoreDescriptionTextObj.text += ".";
        }
        this.scoreDescriptionTextObj.text += "" + scoreExtreme;

        this.gameOverFirstScreen.SetActive(true);

        while(this.gameOverFirstScreen.GetComponent<CanvasGroup>().alpha < 1.0f){
            float speed = 0.5f * Time.deltaTime;

            this.gameOverFirstScreen.GetComponent<CanvasGroup>().alpha += speed;

            yield return new WaitForEndOfFrame();
        }

        this.gameOverFirstScreen.GetComponent<CanvasGroup>().alpha = 1.0f;

        this.waitingForInput = true;

        float timeWaited = 0.0f;
        while(this.waitingForInput && timeWaited < 10.0f)
        {
            timeWaited += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        this.gameOverFinalScreen.SetActive(true);

        while (this.gameOverFinalScreen.GetComponent<CanvasGroup>().alpha < 1.0f)
        {
            float speed = 2.0f * Time.deltaTime;

            this.gameOverFinalScreen.GetComponent<CanvasGroup>().alpha += speed;

            yield return new WaitForEndOfFrame();
        }

        this.gameOverFinalScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
        this.music.source.Stop();
        this.playSound(2);

        yield return new WaitForSeconds(7.5f);

        this.gameOverHideScreen.SetActive(true);

        while (this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha < 1.0f)
        {
            float speed = 0.5f * Time.deltaTime;

            this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha += speed;

            yield return new WaitForEndOfFrame();
        }

        this.gameOverHideScreen.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return null;
    }

    public void createConversation(string raw)
    {
        //Create nodes
        string[] rawLines = raw.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        for(int i = 0; i < rawLines.Length; i++)
        {
            string rawLine = rawLines[i];
            //Debug.Log("raw "+i+": " + rawLine);
            int numberOfTabs = rawLine.Split('\t').Length - 1;

            if (rawLine.Equals(""))
            {
                continue;
            }

            string line = rawLine.Replace("\t", "");
            //Debug.Log(line);

            if (line.StartsWith("//"))
            {
                continue;
            }
            else if (line.StartsWith("-") || line.StartsWith("+"))
            {
                string[] rawHeader = line.Split(new string[]{ ":|:" }, StringSplitOptions.None);
                string content = rawHeader[1];
                string id = "";
                string effect = "";
                if(rawHeader[0].IndexOf(":*:") > -1)
                {
                    string[] rawHeaderData = rawHeader[0].Split(new string[] { ":*:" }, StringSplitOptions.None);
                    id = rawHeaderData[0].Substring(1);
                    effect = rawHeaderData[1];
                }
                else
                {
                    id = rawHeader[0].Substring(1);
                }

                List<string> rawAnswersIds = new List<string>();
                //Debug.Log("searching for answers");
                for(int j = i+1; j < rawLines.Length; j++)
                {
                    string nextLine = rawLines[j];
                    int nextNumberOfTabs = nextLine.Split('\t').Length - 1;
                    //Debug.Log(nextLine);
                    nextLine = nextLine.Replace("\t", "");
                    /*//Debug.Log(j+": "+nextLine);
                    //Debug.Log(numberOfTabs+"/"+nextNumberOfTabs);*/

                    if (nextLine.Equals("") || nextLine.StartsWith("//"))
                    {
                        continue;
                    }
                    else if(nextNumberOfTabs == numberOfTabs + 1)
                    {
                        if(nextLine.StartsWith("=>"))
                        {
                            string[] rawRedirectIds = nextLine.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach(string redirectId in rawRedirectIds){
                                //Debug.Log(id+" => "+redirectId);
                                rawAnswersIds.Add(redirectId);
                            }
                        }
                        else
                        {
                            string nextId = "";
                            string[] nextRawHeader = nextLine.Split(new string[] { ":|:" }, StringSplitOptions.None);
                            if (nextRawHeader[0].IndexOf(":*:") > -1)
                            {
                                string[] nextRawHeaderData = nextRawHeader[0].Split(new string[] { ":*:" }, StringSplitOptions.None);
                                nextId = nextRawHeaderData[0].Substring(1);
                            }
                            else
                            {
                                nextId = nextRawHeader[0].Substring(1);
                            }
                            rawAnswersIds.Add(nextId);
                            //Debug.Log(nextId + " is answer");
                        }
                    }
                    else if(nextNumberOfTabs <= numberOfTabs)
                    {
                        //Debug.Log("break because: "+ numberOfTabs + "/" + nextNumberOfTabs);
                        break;
                    }
                }

                DialogNode newNode = new DialogNode(this, id, content, effect, rawAnswersIds);
                this.nodes.Add(newNode);
            }
            else if (line.StartsWith("=>"))
            {
                continue;
            }
        }
    }

    public DialogNode getNodeById(string searchId)
    {
        DialogNode result = null;

        foreach(DialogNode node in this.nodes)
        {
            if(node.id == ""+ searchId.Trim())
            {
                result = node;
                break;
            }
        }

        return result;
    }

    public void appearText(UnityEngine.UI.Text textObj, string content){
        StartCoroutine(this.appearTextRoutine(textObj, content));
    }

    private IEnumerator appearTextRoutine(UnityEngine.UI.Text textObj, string content)
    {
        textObj.text = "";
        while(textObj.text.Length < content.Length)
        {
            textObj.text += content[textObj.text.Length];
            yield return new WaitForSeconds(0.025f);
        }
    }

    private IEnumerator handleCharacterAnimation()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 10.0f));

        bool continued = true;
        while(continued)
        {
            if(this.characterAnimation == null)
            {
                this.characterAnimation = new SpriteAnimation(this.character, this.idleSprites2, 0, 200);
                StartCoroutine(this.characterAnimation.update());
            }
            else
            {
                if(this.characterAnimation.currentFrame >= this.characterAnimation.sprites.Length)
                {
                    yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 10.0f));
                    this.characterAnimation = null;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator handleCharacterAnimation2()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 10.0f));

        bool continued = true;
        while (continued)
        {
            if (this.characterAnimation2 == null)
            {
                this.characterAnimation2 = new SpriteAnimation(this.character2, this.idleSprites, 0, 200);
                StartCoroutine(this.characterAnimation2.update());
            }
            else
            {
                if (this.characterAnimation2.currentFrame >= this.characterAnimation2.sprites.Length)
                {
                    yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 10.0f));
                    this.characterAnimation2 = null;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void playSound(int id)
    {
        StartCoroutine(this.playSoundRoutine(id));
    }

    private IEnumerator playSoundRoutine(int id)
    {
        GameObject soundSourceObj = (GameObject)Instantiate(this.soundSourcePrefab);
        SoundSource soundSource = soundSourceObj.GetComponent<SoundSource>();

        if(id == 0)
        {
            soundSource.clip = Instantiate(this.defaultSound);
            soundSource.play(false);
        }
        else if(id == 1)
        {
            soundSource.clip = Instantiate(this.winSound);
            soundSource.play(false);
        }
        else if (id == 2)
        {
            soundSource.clip = Instantiate(this.loseSound);
            soundSource.play(false);
        }
        else if (id == 3)
        {
            this.music = soundSource;
            soundSource.clip = Instantiate(this.musicSound);
            soundSource.play(true);
        }
        else if (id == 4)
        {
            soundSource.clip = Instantiate(this.creditsChargeSound);
            soundSource.play(false);
        }
        else if (id == 5)
        {
            soundSource.clip = Instantiate(this.creditsActivateSound);
            soundSource.play(false);
        }
        else if (id == 6)
        {
            soundSource.clip = Instantiate(this.creditsDeploySound);
            soundSource.play(false);
        }

        yield return null;
    }
}
