using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class GameSceneController : MonoBehaviour
{
    FormulaFactory factory = new FormulaFactory();
    #region Declarations
    public GameObject character;
    private float stepX;
    private float stepZ;
    private int step;
    private int speed = 2;

    public Sprite dice0;
    public Sprite dice1;
    public Sprite dice2;
    public Sprite dice3;
    public Sprite dice4;
    public Sprite dice5;
    public Sprite dice6;
    public Button btnRollDice;

    public Button btnTrue;
    public Button btnFalse;
    public Button btnRestart;

    public TextMeshProUGUI _txtFormula;
    public TextMeshProUGUI _txtPhrase;
    public TextMeshProUGUI _txtInterpretationSymbols;
    public TextMeshProUGUI _txtTimer;

    private float startTime;
    private bool isGameRunning;

    public List<Formula> SelectedFormulas;

    public int resultDice = 0;

    #endregion

    void Start()
    {
        step = 0;
        stepX = 1.1f;
        stepZ = 0;

        SelectedFormulas = factory.GetSelectFormulas();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
            UpdateTimerText();
        }

        Vector3 target = character.transform.position;
        target.x = stepX;
        target.z = stepZ;
        character.transform.position = Vector3.MoveTowards(character.transform.position, target, Time.deltaTime * speed);

        if (character.transform.position == target)
        {
            btnRollDice.GetComponent<Image>().sprite = dice0;
        }

        if (step > 0 && step <= SelectedFormulas.Count)
        {
            Formula form = step == SelectedFormulas.Count ?
            SelectedFormulas.LastOrDefault() :
            SelectedFormulas[step];

            _txtFormula.text = form?.FormulaToSolve;
            _txtPhrase.text = form?.Phrase;
            _txtInterpretationSymbols.text = form?.VariableValue;

            Debug.Log("Resposta: " + form.Response + " | Step: " + step);
        }
        else
        {
            _txtFormula.text = "Resolução de fórmulas";
            _txtPhrase.text = "Bem vindo ao nosso game de tabuleiro de lógica com temática Game of Thrones";
            _txtInterpretationSymbols.text = "Divirta-se nessa jornada";
        }

    }


    public string GetTimeStr(bool showMilliseconds)
    {
        float timeElapsed = Time.time - startTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        int milliseconds = Mathf.FloorToInt((timeElapsed * 1000) % 1000);

        string ret = showMilliseconds ? string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds)
        : string.Format("{0:00}:{1:00}", minutes, seconds);

        return ret;
    }

    void UpdateTimerText()
    {
        _txtTimer.text = GetTimeStr(true);
    }

    public void CheckResponse(bool response)
    {
        var formulaCurrent = SelectedFormulas[step];

        if (formulaCurrent.Response == response)
        {
            //Exibe mensagem que acertou
            EditorUtility.DisplayDialog("Você acertou e pode permanecer nessa casa",
             "Acertou!", "Ok");
        }
        else
        {
            //Exibe mensagem que errou
            EditorUtility.DisplayDialog("Você errou e retornará a casa inicial da jogada",
             "Errou :(", "Ok");
            MoveStep(-1 * resultDice, returning: true);
        }

        btnRollDice.enabled = true;
    }

    public void btnTrueClick()
    {
        CheckResponse(true);
    }

    public void btnFalseClick()
    {
        CheckResponse(false);
    }

    public void btnRestartClick()
    {
        _txtFormula.text = "Resolução de fórmulas";
        _txtPhrase.text = "Bem vindo ao nosso game de tabuleiro de lógica com temática Game of Thrones";
        _txtInterpretationSymbols.text = "Divirta-se nessa jornada";

        step = 0;
        stepX = 1.1f;
        stepZ = 0;

        SelectedFormulas = factory.GetSelectFormulas();

        isGameRunning = false;

        _txtTimer.text = "00:00:00";

        btnRollDice.enabled = true;
    }

    public void RollDice()
    {
        if (step == 0)
        {
            startTime = Time.time;
            isGameRunning = true;
        }

        resultDice = UnityEngine.Random.Range(1, 6);
        Debug.Log("Dice rolled: " + resultDice);
        //stepX -= 1.1f * result;
        MoveStep(resultDice, false);
        Debug.Log("Step: " + step);

        // Display sprite graphics
        switch (resultDice)
        {
            case 1:
                btnRollDice.GetComponent<Image>().sprite = dice1;
                break;
            case 2:
                btnRollDice.GetComponent<Image>().sprite = dice2;
                break;
            case 3:
                btnRollDice.GetComponent<Image>().sprite = dice3;
                break;
            case 4:
                btnRollDice.GetComponent<Image>().sprite = dice4;
                break;
            case 5:
                btnRollDice.GetComponent<Image>().sprite = dice5;
                break;
            case 6:
                btnRollDice.GetComponent<Image>().sprite = dice6;
                break;
            default:
                btnRollDice.GetComponent<Image>().sprite = dice0;
                break;
        }
    }

    /// <summary>
    /// Calcula a posição (X, Z) com base no valor de step.
    /// </summary>
    /// <param name="step">O valor do passo que determina a posição.</param>
    /// <returns>Uma tupla contendo as coordenadas (X, Z).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Lançada quando o valor do step está fora do intervalo esperado.</exception>
    private void MoveStep(int resultSteps, bool returning)
    {
        step = step + resultSteps;

        double X = 0;
        double Z = 0;

        if (step == 0)
        {
            // Caso especial para step igual a 0: X = 1.1, Z = 0
            X = 1.1f;
            Z = 0;
        }
        else if (step >= 1 && step <= 9)
        {
            // Intervalo de step de 1 a 9: X = (step - 1) * -1.1, Z = 0
            X = (step - 1) * -1.1f;
            Z = 0;
        }
        else if (step >= 10 && step <= 12)
        {
            // Intervalo de step de 10 a 12: X = -8.8, Z = (step - 9) * 1.1
            X = -8.8f;
            Z = (step - 9) * 1.1f;
        }
        else if (step >= 13 && step <= 20)
        {
            // Intervalo de step de 13 a 20: X = (20 - step) * -1.1, Z = 3.3
            X = (20 - step) * -1.1f;
            Z = 3.3f;
        }
        else if (step >= 21 && step <= 21)
        {
            // Intervalo de step de 21 a 23: X = 0, Z = (step - 17) * 1.1
            X = 0;
            Z = (step - 17) * 1.1f;
        }
        else if (step >= 22 && step <= 30)
        {
            // Intervalo de step de 24 a 31: X = (step - 23) * -1.1, Z = 6.6
            X = (step - 22) * -1.1f;
            Z = 5.5f;
        }
        else if (step == 31)
        {
            X = -8.8f;
            Z = 6.6f;
        }
        else if (step >= 32)
        {
            // Caso especial para step igual a 32: X = -8.8, Z = 7.7
            X = -8.8f;
            Z = 7.7f;

            float timeElapsed = Time.time - startTime;

            EditorUtility.DisplayDialog("Você ganhou o jogo, seu tempo foi: " + GetTimeStr(false),
             "Ganhou!", "Ok");

            isGameRunning = false;
        }
        else
        {
            // Lança uma exceção se o step estiver fora do intervalo esperado
            throw new System.Exception("O valor do step está fora do intervalo esperado.");
        }

        stepX = (float)X;
        stepZ = (float)Z;

        btnRollDice.enabled = returning;
        btnTrue.enabled = !returning;
        btnFalse.enabled = !returning;

    }
}

public enum FormulaType
{
    Proposicional = 0,
    Predicado = 1
}
public class Formula
{
    public Formula(int id,
    string phrase,
    string formulaToSolve,
    string variableValue,
    bool response,
    FormulaType type)
    {
        Id = id;
        Phrase = phrase;
        FormulaToSolve = formulaToSolve;
        VariableValue = variableValue;
        Response = response;
        Type = type;
    }

    public int Id { get; set; }
    public string Phrase { get; set; }
    public string FormulaToSolve { get; set; }
    public string VariableValue { get; set; }
    public bool Response { get; set; }
    public FormulaType Type { get; set; }
}

public class FormulaFactory
{
    public List<Formula> GetSelectFormulas()
    {

        System.Random random = new System.Random();
        var idsProp = Enumerable.Range(1, 25).OrderBy(x => random.Next()).Take(16).ToList();

        var formulasProp = GetAllFormulas().Where(x => idsProp.Contains(x.Id)).ToList();

        List<Formula> SelectedFormulas = new List<Formula>();
        SelectedFormulas.AddRange(formulasProp);

        var idsPred = Enumerable.Range(26, 25).OrderBy(x => random.Next()).Take(16).ToList();
        var formulasPred = GetAllFormulas().Where(x => idsPred.Contains(x.Id)).ToList();

        SelectedFormulas.AddRange(formulasPred);

        return SelectedFormulas;

    }
    public List<Formula> GetAllFormulas()
    {

        List<Formula> formulas = new List<Formula>
        {
            new Formula(1, "Você chegou a Winterfell e para entrar no castelo deve resolver a seguinte fórmula:", "P ^ Q", "P = T, Q = F", false, FormulaType.Proposicional),
            new Formula(2, "Você encontrou um Lannister, para ele pagar suas dívidas, resolva a fórmula:", "~P v Q", "P = F, Q = T", true, FormulaType.Proposicional),
            new Formula(3, "Você está na Muralha, a noite é escura e cheia de terrores. Resolva a fórmula para se proteger:", "P → (Q ^ R)", "P = T, Q = F, R = T", false, FormulaType.Proposicional),
            new Formula(4, "No jogo dos tronos, você vence ou morre. Para continuar vivo, resolva a fórmula:", "(P ^ ~Q) v (R → S)", "P = T, Q = F, R = T, S = F", true, FormulaType.Proposicional),
            new Formula(5, "Chaos é uma escada. Para subir, resolva a fórmula:", "(P v ~Q) ^ (R v S)", "P = T, Q = F, R = F, S = T", true, FormulaType.Proposicional),
            new Formula(6, "Todos os homens devem morrer. Para se salvar, resolva a fórmula:", "(P <-> Q) → (R ^ S)", "P = T, Q = F, R = T, S = F", false, FormulaType.Proposicional),
            new Formula(7, "Para fazer algo por amor, resolva a seguinte fórmula:", "~(P ^ Q) ^ (R v ~S)", "P = T, Q = F, R = F, S = T", true, FormulaType.Proposicional),
            new Formula(8, "Você sabe nada, Jon Snow. Para provar o contrário, resolva a fórmula:", "(P v Q) ^ (~R → S)", "P = F, Q = T, R = T, S = F", false, FormulaType.Proposicional),
            new Formula(9, "Fogo e Sangue. Para sobreviver, resolva a fórmula:", "(~P ^ ~Q) v (R ^ ~S)", "P = F, Q = F, R = T, S = F", true, FormulaType.Proposicional),
            new Formula(10, "Segure a porta! Para mantê-la fechada, resolva a fórmula:", "(P v (P ^ Q)) → (R v ~S)", "P = T, Q = F, R = F, S = F", true, FormulaType.Proposicional),
            new Formula(11, "Eu bebo e sei coisas. Para provar seu conhecimento, resolva a fórmula:", "~(P v Q) ^ (R → S)", "P = F, Q = T, R = T, S = F", false, FormulaType.Proposicional),
            new Formula(12, "Nós não semeamos. Para continuar, resolva a fórmula:", "P → (~Q v (R ^ S))", "P = T, Q = F, R = T, S = F", true, FormulaType.Proposicional),
            new Formula(13, "Winterfell é meu. Para manter o castelo, resolva a fórmula:", "~P → (Q ^ (R v ~S))", "P = F, Q = T, R = F, S = T", true, FormulaType.Proposicional),
            new Formula(14, "Nossa é a Fúria. Para liberar sua fúria, resolva a fórmula:", "(P ^ (P v Q)) ^ (~R v S)", "P = T, Q = F, R = F, S = T", true, FormulaType.Proposicional),
            new Formula(15, "O Norte se lembra. Para provar isso, resolva a fórmula:", "((~P) ^ (P v Q)) → (R ^ S)", "P = F, Q = T, R = T, S = F", true, FormulaType.Proposicional),
            new Formula(16, "Ouça-me rugir! Para rugir alto, resolva a fórmula:", "P v ((~Q) ^ (R v S))", "P = T, Q = F, R = F, S = T", true, FormulaType.Proposicional),
            new Formula(17, "Não curvados, não quebrados, não submissos. Para continuar de pé, resolva a fórmula:", "((~P) v Q) ^ (R → ~S)", "P = F, Q = T, R = T, S = F", true, FormulaType.Proposicional),
            new Formula(18, "Crescendo Fortes. Para crescer, resolva a fórmula:", "P ^ Q ^ (R v ~S)", "P = F, Q = F, R = T, S = T", false, FormulaType.Proposicional),
            new Formula(19, "Uma garota não tem nome. Para provar sua identidade, resolva a fórmula:", "(P v Q) ^ ((~R) → S)", "P = F, Q = T, R = T, S = F", false, FormulaType.Proposicional),
            new Formula(20, "O rei no norte! Para manter seu título, resolva a fórmula:", "~(P ^ Q) ^ (R v S)", "P = T, Q = F, R = F, S = T", true, FormulaType.Proposicional),
            new Formula(21, "Um leão não se preocupa com a opinião de ovelhas. Para manter sua confiança, resolva a fórmula:", "P ^ (~Q ^ (R → S))", "P = T, Q = F, R = T, S = F", false, FormulaType.Proposicional),
            new Formula(22, "Todos os homens devem servir. Para mostrar sua servidão, resolva a fórmula:", "P → (Q v (~R ^ S))", "P = T, Q = F, R = T, S = F", false, FormulaType.Proposicional),
            new Formula(23, "O que está morto não pode morrer. Para provar a força dos mortos, resolva a fórmula:", "((~P) → Q) ^ (R v ~S)", "P = F, Q = T, R = F, S = T", true, FormulaType.Proposicional),
            new Formula(24, "Você ganha ou morre. Para ganhar, resolva a fórmula:", "P <-> (~Q ^ (R → S))", "P = T, Q = F, R = T, S = F", false, FormulaType.Proposicional),
            new Formula(25, "A noite é escura e cheia de terrores. Para afastar o terror, resolva a fórmula:", "P v (~Q ^ (R v S))", "P = T, Q = F, R = F, S = T", true, FormulaType.Proposicional),
            new Formula(26, "Você chegou a Porto Real e precisa resolver a fórmula para entrar na Fortaleza Vermelha:", "∀x (P(x) → Q(x))", "P(Dragão) = T, Q(Dragão) = F", false, FormulaType.Predicado),
            new Formula(27, "Você encontrou o exército dos mortos. Resolva a fórmula para escapar:", "∃x (P(x) ^ ~Q(x))", "P(Jon) = T, Q(Jon) = F", true, FormulaType.Predicado),
            new Formula(28, "Você está em Meereen. Para continuar, resolva a fórmula:", "∀x (P(x) v Q(x))", "P(Daenerys) = F, Q(Daenerys) = T", true, FormulaType.Predicado),
            new Formula(29, "No meio do caminho, você encontra uma feiticeira. Resolva a fórmula para não ser enfeitiçado:", "∃x (~P(x) ^ R(x))", "P(Melisandre) = F, R(Melisandre) = T", true, FormulaType.Predicado),
            new Formula(30, "Você precisa atravessar o Mar Estreito. Resolva a fórmula para não se afogar:", "∀x (P(x) → (Q(x) ^ R(x)))", "P(Navio) = T, Q(Navio) = T, R(Navio) = F", false, FormulaType.Predicado),
            new Formula(31, "Você encontrou Arya Stark. Resolva a fórmula para não ser atacado:", "∃x (P(x) v (Q(x) → ~R(x)))", "P(Arya) = T, Q(Arya) = F, R(Arya) = T", true, FormulaType.Predicado),
            new Formula(32, "Você está na casa dos Imortais. Resolva a fórmula para encontrar a saída:", "∀x (P(x) → ~Q(x))", "P(Magia) = T, Q(Magia) = T", false, FormulaType.Predicado),
            new Formula(33, "Você encontrou um dragão. Resolva a fórmula para não ser queimado:", "∃x (P(x) ^ (Q(x) v R(x)))", "P(Dragão) = T, Q(Dragão) = F, R(Dragão) = T", true, FormulaType.Predicado),
            new Formula(34, "Você está em Volantis. Resolva a fórmula para conseguir um barco:", "∀x (~P(x) ^ (Q(x) → R(x)))", "P(Barco) = F, Q(Barco) = T, R(Barco) = F", false, FormulaType.Predicado),
            new Formula(35, "Você encontrou um corvo de três olhos. Resolva a fórmula para receber uma visão:", "∃x (P(x) ^ Q(x) ^ ~R(x))", "P(Corvo) = T, Q(Corvo) = T, R(Corvo) = F", true, FormulaType.Predicado),
            new Formula(36, "Você está na Cidade Livre de Braavos. Resolva a fórmula para entrar na Casa do Preto e Branco:", "∀x (P(x) v (Q(x) ^ ~R(x)))", "P(Portão) = F, Q(Portão) = T, R(Portão) = F", true, FormulaType.Predicado),
            new Formula(37, "Você encontrou um grupo de selvagens. Resolva a fórmula para ganhar a confiança deles:", "∃x (P(x) ^ (~Q(x) v R(x)))", "P(Tormund) = T, Q(Tormund) = F, R(Tormund) = T", true, FormulaType.Predicado),
            new Formula(38, "Você está em Dorne. Resolva a fórmula para atravessar o deserto:", "∀x (P(x) → (~Q(x) ^ R(x)))", "P(Deserto) = T, Q(Deserto) = T, R(Deserto) = T", false, FormulaType.Predicado),
            new Formula(39, "Você encontrou um grupo de mercenários. Resolva a fórmula para contratá-los:", "∃x (P(x) ^ (Q(x) → R(x)))", "P(Mercenário) = T, Q(Mercenário) = T, R(Mercenário) = F", false, FormulaType.Predicado),
            new Formula(40, "Você está em Jardim de Cima. Resolva a fórmula para se proteger dos Espinhos:", "∀x (P(x) ^ Q(x) ^ R(x))", "P(Olenna) = T, Q(Olenna) = F, R(Olenna) = T", false, FormulaType.Predicado),
            new Formula(41, "Você encontrou um lobisomem. Resolva a fórmula para não ser atacado:", "∃x (P(x) v Q(x) v ~R(x))", "P(Lobo) = F, Q(Lobo) = F, R(Lobo) = T", false, FormulaType.Predicado),
            new Formula(42, "Você está em Harrenhal. Resolva a fórmula para não ser capturado:", "∀x (P(x) v (~Q(x) ^ R(x)))", "P(Torre) = T, Q(Torre) = F, R(Torre) = F", true, FormulaType.Predicado),
            new Formula(43, "Você encontrou um homem sem rosto. Resolva a fórmula para não ser morto:", "∃x (~P(x) ^ Q(x) ^ R(x))", "P(Homem) = F, Q(Homem) = T, R(Homem) = T", true, FormulaType.Predicado),
            new Formula(44, "Você está em Pedra do Dragão. Resolva a fórmula para encontrar a espada de aço valiriano:", "∀x (P(x) ^ Q(x) v ~R(x))", "P(Espada) = T, Q(Espada) = F, R(Espada) = T", false, FormulaType.Predicado),
            new Formula(45, "Você encontrou os Filhos da Floresta. Resolva a fórmula para ganhar seus poderes:", "∃x (P(x) ^ (Q(x) v ~R(x)))", "P(Filho) = T, Q(Filho) = T, R(Filho) = F", true, FormulaType.Predicado),
            new Formula(46, "Você está em Qarth. Resolva a fórmula para ganhar a confiança dos Treze:", "∀x (P(x) → (Q(x) ^ ~R(x)))", "P(Confiança) = T, Q(Confiança) = F, R(Confiança) = T", false, FormulaType.Predicado),
            new Formula(47, "Você encontrou um sacerdote vermelho. Resolva a fórmula para receber uma bênção:", "∃x (P(x) ^ ~Q(x) ^ R(x))", "P(Sacerdote) = T, Q(Sacerdote) = F, R(Sacerdote) = T", true, FormulaType.Predicado),
            new Formula(48, "Você está em Atalaia da Água Cinzenta. Resolva a fórmula para encontrar os Reed:", "∀x (~P(x) ^ Q(x) ^ R(x))", "P(Jojen) = F, Q(Jojen) = T, R(Jojen) = T", true, FormulaType.Predicado),
            new Formula(49, "Você encontrou um Caminhante Branco. Resolva a fórmula para derrotá-lo:", "∃x (P(x) ^ (~Q(x) v R(x)))", "P(Caminhante) = T, Q(Caminhante) = F, R(Caminhante) = F", true, FormulaType.Predicado),
            new Formula(50, "Você está em Vila Toupeira. Resolva a fórmula para conseguir suprimentos:", "∀x (P(x) ^ Q(x) → R(x))", "P(Suprimentos) = T, Q(Suprimentos) = T, R(Suprimentos) = F", false, FormulaType.Predicado)
        };

        return formulas;
    }
}
