﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Keysget : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = GameManager.Instance.keys.ToString();
    }
}
