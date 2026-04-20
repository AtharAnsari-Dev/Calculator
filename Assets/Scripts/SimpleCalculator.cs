using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class SimpleCalculator : MonoBehaviour
{
    public TMP_InputField expressionInput; 

// For input from users
    public void AppendInput(string value)
    {
        //if (expressionInput.text == "Error")
            //expressionInput.text = "";
            
        expressionInput.text += value;
    }

    public void ClearResult()
    {
        expressionInput.text = "";
    }

    public void Calculate()
    {
        if (string.IsNullOrEmpty(expressionInput.text))
            return;

        //try
        {
            float result = EvaluateExpression(expressionInput.text);
            
            if (float.IsInfinity(result) || float.IsNaN(result))
                expressionInput.text = "Error";
            else
                expressionInput.text = result.ToString();
        }
        //catch (System.Exception)
        //{
            //expressionInput.text = "Error";
        //}
    }

    private float EvaluateExpression(string expression)
    {
        expression = expression.Replace(" ", "");
        
        List<string> tokens = new List<string>();
        string currentNum = "";
        
        for (int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];
        
            if (char.IsDigit(c) || c == '.' || 
                (c == '-' && (i == 0 || "+-*/".Contains(expression[i - 1].ToString()))))
            {
                currentNum += c;
            }
            else if ("+-*/".Contains(c.ToString()))
            {
                if (!string.IsNullOrEmpty(currentNum))
                {
                    tokens.Add(currentNum);
                    currentNum = "";
                }
                tokens.Add(c.ToString());
            }
        }
        
        if (!string.IsNullOrEmpty(currentNum))
        {
            tokens.Add(currentNum);
        }

        for (int i = 1; i < tokens.Count - 1; i++) 
        {
            if (tokens[i] == "*" || tokens[i] == "/")
            {
                float left = float.Parse(tokens[i - 1]);
                float right = float.Parse(tokens[i + 1]);
                float result = tokens[i] == "*" ? left * right : left / right;
                
                tokens[i - 1] = result.ToString(); // Replace left num with result
                tokens.RemoveRange(i, 2);          // Remove operator and right num
                i--; 
            }
        }

        for (int i = 1; i < tokens.Count - 1; i++)
        {
            if (tokens[i] == "+" || tokens[i] == "-")
            {
                float left = float.Parse(tokens[i - 1]);
                float right = float.Parse(tokens[i + 1]);
                float result = tokens[i] == "+" ? left + right : left - right;
                
                tokens[i - 1] = result.ToString();
                tokens.RemoveRange(i, 2);
                i--;
            }
        }

        if (tokens.Count > 0)
        {
            return float.Parse(tokens[0]);
        }
        
        return 0;
    }
}
