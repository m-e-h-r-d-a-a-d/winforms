﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace ScratchProject;

// As we can't currently design in VS in the runtime solution, mark as "Default" so this opens in code view by default.
[DesignerCategory("Default")]
public partial class Form1 : Form
{
    private RichTextBox _textBox;

    public Form1()
    {
        InitializeComponent();

        _textBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            EnableAutoDragDrop = true
        };

        Controls.Add(_textBox);
    }
}
