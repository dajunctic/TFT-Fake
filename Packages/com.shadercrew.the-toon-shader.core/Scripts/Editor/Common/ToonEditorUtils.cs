using ShaderCrew.TheToonShader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.TheToonShader
{
    public static class ToonEditorUtils
    {

        public static void DrawBox(Rect position, Color color)
        {
            Color oldColor = GUI.color;
            GUI.color = color;
            GUI.Box(position, GUIContent.none);
            GUI.color = oldColor;
        }


        public static bool MakeTheToonShaderSectionHeaderWithFoldout(string name, bool showContent)
        {

            //showDescription = EditorGUILayout.Foldout(showDescription, name, EditorStyles.boldLabel); //, EditorStyles.boldLabel);

            Rect rect2 = EditorGUILayout.BeginVertical();

            rect2.width = rect2.width + rect2.x;
            rect2.x = 0;
            if (EditorGUIUtility.isProSkin)
            {
                Color lightBlue = new Color(1.5f, 1.6f, 1.7f, 2);
                //EditorUtils.DrawUILine(new Color(0.2f, 0.2f, 0.2f, 1f));
                //EditorUtils.DrawUILine(rect2, lightBlue, 1, 0);
                EditorUtils.DrawUILineBottom(rect2, new Color(0.25f, 0.25f, 0.25f, 1f));

            }
            else
            {
                Color lightBlue = new Color(0.6f, 0.8f, 2f, 2);
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
                EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            }

            //EditorGUILayout.Space();
            if (EditorGUIUtility.isProSkin)
            {
                //GUI.Box(rect2, GUIContent.none);
                DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1.5f));
            }
            else
            {

                DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1.5f));
                //DrawBox(rect2, new Color(0.8f, 0.8f, 0.8f, 1));
            }

            EditorGUILayout.Space(3f);
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = EditorStyles.foldout.fontSize + 2;
            Color myStyleColor = new Color(0.8f, 0.95f, 1f, 1f);// *0.9f;

            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;

            showContent = EditorGUILayout.Foldout(showContent, name, true, style); //, EditorStyles.boldLabel);

            style.fontStyle = previousStyle;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();

            //EditorGUILayout.Space();
            EditorGUILayout.Space(3f);
            if (EditorGUIUtility.isProSkin)
            {

                Color lightBlue = new Color(1.1f, 1.2f, 1.7f, 0.3f);
                EditorUtils.DrawUILineBottom(rect2, lightBlue, 1);
            }
            else
            {
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            }

            EditorGUILayout.EndVertical();


            return showContent;
        }
        struct DoubleBool
        {
            bool one;
            bool two;
        }
        public static Tuple<bool, bool> MakeTheToonShaderSectionHeaderWithFoldoutWithToggle(string name, bool showContent, bool toggle)
        {

            //showDescription = EditorGUILayout.Foldout(showDescription, name, EditorStyles.boldLabel); //, EditorStyles.boldLabel);

            Rect rect2 = EditorGUILayout.BeginVertical();

            rect2.width = rect2.width + rect2.x;
            rect2.x = 0;
            if (EditorGUIUtility.isProSkin)
            {
                Color lightBlue = new Color(1.5f, 1.6f, 1.7f, 2);
                //EditorUtils.DrawUILine(new Color(0.2f, 0.2f, 0.2f, 1f));
                //EditorUtils.DrawUILine(rect2, lightBlue, 1, 0);
                EditorUtils.DrawUILineBottom(rect2, new Color(0.25f, 0.25f, 0.25f, 1f));


            }
            else
            {
                Color lightBlue = new Color(0.6f, 0.8f, 2f, 2);
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
                EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            }

            //EditorGUILayout.Space();
            if (EditorGUIUtility.isProSkin)
            {
                //GUI.Box(rect2, GUIContent.none);
                DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1.5f));
            }
            else
            {

                DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1.5f));
                //DrawBox(rect2, new Color(0.8f, 0.8f, 0.8f, 1));
            }

            EditorGUILayout.Space(3f);
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = EditorStyles.foldout.fontSize + 2;
            Color myStyleColor = new Color(0.8f, 0.95f, 1f, 1f);// *0.9f;

            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;
            EditorGUILayout.BeginHorizontal();
            showContent = EditorGUILayout.Foldout(showContent, name, true, style); //, EditorStyles.boldLabel);
                                                                                   //GUILayout.FlexibleSpace();
                                                                                   //enableShadingStyling.floatValue = Convert.ToSingle(EditorGUILayout.Toggle(Convert.ToBoolean(enableShadingStyling.floatValue), GUILayout.Width(15)));
            toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();
            style.fontStyle = previousStyle;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();

            //EditorGUILayout.Space();
            EditorGUILayout.Space(3f);
            if (EditorGUIUtility.isProSkin)
            {
                //EditorUtils.DrawUILineBottom(rect2, new Color(0.25f, 0.25f, 0.25f, 1f));
                Color lightBlue = new Color(1.1f, 1.2f, 1.7f, 0.5f);
                EditorUtils.DrawUILineBottom(rect2, lightBlue, 1);
            }
            else
            {
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            }

            EditorGUILayout.EndVertical();


            return Tuple.Create(showContent, toggle);
        }

        //bool MakeTheToonShaderSectionHeaderWithFoldoutWithToggle(string name, bool showContent, bool toggle)
        public static bool MakeTheToonShaderSectionHeaderWithFoldoutWithToggle(GUIContent guiContent, SavedBool foldoutBool, MaterialProperty toggle)
        {

            if (toggle.floatValue == 0)
            {
                foldoutBool.value = false;
            }

            //showDescription = EditorGUILayout.Foldout(showDescription, name, EditorStyles.boldLabel); //, EditorStyles.boldLabel);

            Rect rect2 = EditorGUILayout.BeginVertical();

            rect2.width = rect2.width + rect2.x;
            rect2.x = 0;
            if (EditorGUIUtility.isProSkin)
            {
                Color lightBlue = new Color(1.5f, 1.6f, 1.7f, 2);
                //EditorUtils.DrawUILine(new Color(0.2f, 0.2f, 0.2f, 1f));
                //EditorUtils.DrawUILine(rect2, lightBlue, 1, 0);
                EditorUtils.DrawUILineBottom(rect2, new Color(0.25f, 0.25f, 0.25f, 1f));


            }
            else
            {
                Color lightBlue = new Color(0.6f, 0.8f, 2f, 2);
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
                EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            }

            //EditorGUILayout.Space();
            if (EditorGUIUtility.isProSkin)
            {
                //GUI.Box(rect2, GUIContent.none);
                DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1.5f));
            }
            else
            {

                DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1.5f));
                //DrawBox(rect2, new Color(0.8f, 0.8f, 0.8f, 1));
            }

            EditorGUILayout.Space(3f);
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = EditorStyles.foldout.fontSize + 2;
            Color myStyleColor = new Color(0.8f, 0.95f, 1f, 1f);// *0.9f;

            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;
            EditorGUILayout.BeginHorizontal();
            foldoutBool.value = EditorGUILayout.Foldout(foldoutBool.value, guiContent, true, style);
            //GUILayout.FlexibleSpace();
            //enableShadingStyling.floatValue = Convert.ToSingle(EditorGUILayout.Toggle(Convert.ToBoolean(enableShadingStyling.floatValue), GUILayout.Width(15)));
            bool toggleTMP = EditorGUILayout.Toggle(Convert.ToBoolean(toggle.floatValue), GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();
            style.fontStyle = previousStyle;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();

            //EditorGUILayout.Space();
            EditorGUILayout.Space(3f);
            if (EditorGUIUtility.isProSkin)
            {
                //EditorUtils.DrawUILineBottom(rect2, new Color(0.25f, 0.25f, 0.25f, 1f));
                Color lightBlue = new Color(1.1f, 1.2f, 1.7f, 0.5f);
                EditorUtils.DrawUILineBottom(rect2, lightBlue, 1);
            }
            else
            {
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            }

            EditorGUILayout.EndVertical();

            float toggleFloat = Convert.ToSingle(toggleTMP);
            if (toggle.floatValue != toggleFloat && toggleFloat == 1)
            {
                foldoutBool.value = true;
            }
            toggle.floatValue = toggleFloat;

            return (foldoutBool.value && toggle.floatValue == 1);
        }







        public static bool MakeTheToonShaderSubSectionHeaderWithFoldout(GUIContent guiContent, bool showContent, Color color, int plusTextSize = 0)
        {

            //showDescription = EditorGUILayout.Foldout(showDescription, name, EditorStyles.boldLabel); //, EditorStyles.boldLabel);

            Rect rect2 = EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(1f);
            //Color backgroundColor = color; 
            //EditorUtils.DrawUILine(rect2, new Color(0.3f, 0.3f, 0.3f, 1f),2);
            DrawBox(rect2, color);
            //if (EditorGUIUtility.isProSkin)
            //{
            //    Color lightBlue = new Color(0.5f, 0.6f, 0.7f, 1);
            //    //EditorUtils.DrawUILine(new Color(0.2f, 0.2f, 0.2f, 1f));
            //    EditorUtils.DrawUILine(lightBlue, 1, 0);

            //}
            //else
            //{
            //    Color lightBlue = new Color(0.6f, 0.8f, 2f, 1);
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //    EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            //}

            ////EditorGUILayout.Space();
            //if (EditorGUIUtility.isProSkin)
            //{
            //    //GUI.Box(rect2, GUIContent.none);
            //DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1f));
            //}
            //else
            //{

            //   DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1f));
            //    //DrawBox(rect2, new Color(0.8f, 0.8f, 0.8f, 1));
            //}
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = EditorStyles.foldout.fontSize + 1 + plusTextSize;
            Color myStyleColor = new Color(1f, 1f, 1f, 1f);
            Color hoverColor = new Color(0f, 1f, 0.5f, 1f);
            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;
            showContent = EditorGUILayout.Foldout(showContent, guiContent, style); //, EditorStyles.boldLabel);
            style.fontStyle = previousStyle;
            style = EditorStyles.foldout;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();



            //if (EditorGUIUtility.isProSkin)
            //{
            //    EditorUtils.DrawUILine(new Color(0.25f, 0.25f, 0.25f, 1f));
            //}
            //else
            //{
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //}
            //if(showContent)
            //{

            EditorUtils.DrawUILineBottom(rect2, new Color(0.7f, 0.7f, 0.7f, 1f), 1, 0);

            //}
            EditorGUILayout.EndVertical();
            //EditorGUILayout.Space();

            return showContent;
        }



        public static bool MakeTheToonShaderSubSectionHeaderWithFoldout(GUIContent guiContent, bool showContent, Color color, int plusTextSize = 0, params SerializedProperty[] serializedProperties)
        {

            int count = 0;
            int totalCount = 0;
            foreach (SerializedProperty serializedProperty in serializedProperties)
            {
                if (serializedProperty.name.Contains("_"))
                {
                    totalCount++;
                    if (serializedProperty.boolValue == true)
                    {
                        count++;
                        //Debug.Log(" : " +  serializedProperty.name);
                    }
                }

            }


            Rect rect2 = EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(1f);

            DrawBox(rect2, color);

            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = EditorStyles.foldout.fontSize + 1 + plusTextSize;
            Color myStyleColor = new Color(1f, 1f, 1f, 1f);
            Color hoverColor = new Color(0f, 1f, 0.5f, 1f);
            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;

            EditorGUILayout.BeginHorizontal();
            showContent = EditorGUILayout.Foldout(showContent, guiContent, style); //, EditorStyles.boldLabel);
            int tempIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            //string spaceString = "    ";
            string spaceString = "    ";
            if (count > 9)
            {
                spaceString = "";
            }
            EditorGUILayout.LabelField(spaceString + count + " / " + totalCount, GUILayout.Width(50));
            EditorGUI.indentLevel = tempIndent;
            EditorGUILayout.EndHorizontal();

            style.fontStyle = previousStyle;
            style = EditorStyles.foldout;


            EditorUtils.DrawUILineBottom(rect2, new Color(0.7f, 0.7f, 0.7f, 1f), 1, 0);


            EditorGUILayout.EndVertical();


            return showContent;
        }

        public static bool MakeTheToonShaderSubSectionHeaderWithFoldout(GUIContent guiContent, bool showContent, int plusTextSize = 0)
        {

            //showDescription = EditorGUILayout.Foldout(showDescription, name, EditorStyles.boldLabel); //, EditorStyles.boldLabel);

            Rect rect2 = EditorGUILayout.BeginVertical();
            Color backgroundColor = new Color(0.7f, 0.7f, 0.7f, 0.9f);
            DrawBox(rect2, backgroundColor);
            //if (EditorGUIUtility.isProSkin)
            //{
            //    Color lightBlue = new Color(0.5f, 0.6f, 0.7f, 1);
            //    //EditorUtils.DrawUILine(new Color(0.2f, 0.2f, 0.2f, 1f));
            //    EditorUtils.DrawUILine(lightBlue, 1, 0);

            //}
            //else
            //{
            //    Color lightBlue = new Color(0.6f, 0.8f, 2f, 1);
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //    EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            //}

            ////EditorGUILayout.Space();
            //if (EditorGUIUtility.isProSkin)
            //{
            //    //GUI.Box(rect2, GUIContent.none);
            //DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1f));
            //}
            //else
            //{

            //   DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1f));
            //    //DrawBox(rect2, new Color(0.8f, 0.8f, 0.8f, 1));
            //}
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = EditorStyles.foldout.fontSize + 1 + plusTextSize;
            Color myStyleColor = new Color(1f, 1f, 1f, 1f);
            Color hoverColor = new Color(0f, 1f, 0.5f, 1f);
            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;
            showContent = EditorGUILayout.Foldout(showContent, guiContent, style); //, EditorStyles.boldLabel);
            style.fontStyle = previousStyle;
            style = EditorStyles.foldout;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();



            //if (EditorGUIUtility.isProSkin)
            //{
            //    EditorUtils.DrawUILine(new Color(0.25f, 0.25f, 0.25f, 1f));
            //}
            //else
            //{
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //}

            EditorGUILayout.EndVertical();


            return showContent;
        }










        public static bool MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(GUIContent guiContent, SavedBool foldoutBool, MaterialProperty toggle, Color color, int plusTextSize = 0)
        {

            if (toggle.floatValue == 0)
            {
                foldoutBool.value = false;
            }
            Rect rect2 = EditorGUILayout.BeginVertical();

            DrawBox(rect2, color);

            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = EditorStyles.foldout.fontSize + 1 + plusTextSize;
            Color myStyleColor = new Color(1f, 1f, 1f, 1f);
            Color hoverColor = new Color(0f, 1f, 0.5f, 1f);
            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;

            EditorGUILayout.BeginHorizontal();
            foldoutBool.value = EditorGUILayout.Foldout(foldoutBool.value, guiContent, true, style);

            bool toggleTMP = EditorGUILayout.Toggle(Convert.ToBoolean(toggle.floatValue), GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();


            style.fontStyle = previousStyle;
            style = EditorStyles.foldout;

            //}
            EditorUtils.DrawUILineBottom(rect2, new Color(0.7f, 0.7f, 0.7f, 1f), 1, 0);
            EditorGUILayout.EndVertical();


            //foldoutBool.value = foldoutBool.value;

            float toggleFloat = Convert.ToSingle(toggleTMP);
            if (toggle.floatValue != toggleFloat && toggleFloat == 1)
            {
                foldoutBool.value = true;
            }
            toggle.floatValue = toggleFloat;

            return (foldoutBool.value && toggle.floatValue == 1);


        }






        public static bool MakeTheToonShaderSectionHeaderWithFoldoutWithToggle(GUIContent guiContent, SerializedProperty foldoutBool, SerializedProperty toggle)
        {


            if (toggle.boolValue == false)
            {
                foldoutBool.boolValue = false;
            }

            Rect rect2 = EditorGUILayout.BeginVertical();

            //rect2.width = rect2.width + rect2.x;
            //rect2.x = 0;
            if (EditorGUIUtility.isProSkin)
            {
                Color lightBlue = new Color(1.5f, 1.6f, 1.7f, 2);
                EditorUtils.DrawUILineBottom(rect2, new Color(0.25f, 0.25f, 0.25f, 1f));
            }
            else
            {
                Color lightBlue = new Color(0.6f, 0.8f, 2f, 2);
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
                EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            }

            if (EditorGUIUtility.isProSkin)
            {
                DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1.5f));
            }
            else
            {
                DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1.5f));
            }

            EditorGUILayout.Space(3f);
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = EditorStyles.foldout.fontSize + 2;
            Color myStyleColor = new Color(0.8f, 0.95f, 1f, 1f);// *0.9f;

            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;
            EditorGUILayout.BeginHorizontal();
            foldoutBool.boolValue = EditorGUILayout.Foldout(foldoutBool.boolValue, guiContent, true, style);
            //GUILayout.FlexibleSpace();
            //enableShadingStyling.floatValue = Convert.ToSingle(EditorGUILayout.Toggle(Convert.ToBoolean(enableShadingStyling.floatValue), GUILayout.Width(15)));

            bool toggleTMP = EditorGUILayout.Toggle(toggle.boolValue, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();
            style.fontStyle = previousStyle;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();

            //EditorGUILayout.Space();
            EditorGUILayout.Space(3f);
            if (EditorGUIUtility.isProSkin)
            {
                //EditorUtils.DrawUILineBottom(rect2, new Color(0.25f, 0.25f, 0.25f, 1f));
                Color lightBlue = new Color(1.1f, 1.2f, 1.7f, 0.5f);
                EditorUtils.DrawUILineBottom(rect2, lightBlue, 1);
            }
            else
            {
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            }

            EditorGUILayout.EndVertical();


            if (toggle.boolValue != toggleTMP && toggleTMP)
            {
                foldoutBool.boolValue = true;
            }
            toggle.boolValue = toggleTMP;

            return (foldoutBool.boolValue && toggle.boolValue);
        }

        private static int CalculateWidthFromPropertyLengthAndEnabled(int enabledCount, params SerializedProperty[] serializedProperties)
        {
            int width;
            int serializedPropertiesArrayLength = serializedProperties.Length;
            if (serializedPropertiesArrayLength >= 100)
            {
                if (enabledCount >= 100)
                {
                    width = 56;

                }
                else if (enabledCount >= 10)
                {
                    width = 51;

                }
                else
                {
                    width = 44;

                }

            }
            else if (serializedPropertiesArrayLength >= 10 && serializedPropertiesArrayLength < 100)
            {
                if (enabledCount >= 10)
                {
                    width = 47;

                }
                else
                {
                    width = 40;

                }
            }
            else
            {
                width = 30;
            }

            return width;

        }

        public static bool MakeTheToonShaderSectionHeaderWithFoldoutWithToggle(GUIContent guiContent,
                                                                                SerializedProperty foldoutBool,
                                                                                SerializedProperty toggle,
                                                                                params SerializedProperty[] serializedPropertiesArray)
        {


            if (toggle.boolValue == false)
            {
                foldoutBool.boolValue = false;
                SetSerializedPropertiesBool(false, serializedPropertiesArray);
            }

            Rect rect2 = EditorGUILayout.BeginVertical();

            rect2.width = rect2.width + rect2.x;
            rect2.x = 0;
            if (EditorGUIUtility.isProSkin)
            {
                Color lightBlue = new Color(1.5f, 1.6f, 1.7f, 2);
                EditorUtils.DrawUILineBottom(rect2, new Color(0.25f, 0.25f, 0.25f, 1f));
            }
            else
            {
                Color lightBlue = new Color(0.6f, 0.8f, 2f, 2);
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
                EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            }

            if (EditorGUIUtility.isProSkin)
            {
                if (toggle.boolValue)
                {
                    DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1.5f));
                    //DrawBox(rect2, Color.black);
                }
                else
                {
                    DrawBox(rect2, new Color(2.5f, 0.5f, 0.5f, 1.5f));
                }
            }
            else
            {
                DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1.5f));
            }

            EditorGUILayout.Space(3f);
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.Bold;
            //style.fontSize = EditorStyles.foldout.fontSize + 2;
            style.fontSize = EditorStyles.foldout.fontSize + 1;
            Color myStyleColor = new Color(0.8f, 0.95f, 1f, 1f);// *0.9f;
            //myStyleColor = Color.white;

            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;
            EditorGUILayout.BeginHorizontal();
            foldoutBool.boolValue = EditorGUILayout.Foldout(foldoutBool.boolValue, guiContent, true, style);
            //GUILayout.FlexibleSpace();
            //enableShadingStyling.floatValue = Convert.ToSingle(EditorGUILayout.Toggle(Convert.ToBoolean(enableShadingStyling.floatValue), GUILayout.Width(15)));

            //int count = 0;
            //foreach (SerializedProperty serializedProperty in serializedPropertiesArray)
            //{
            //    if (serializedProperty.boolValue == true)
            //    {
            //        count++;
            //    }
            //}

            int count = 0;
            int totalCount = 0;
            foreach (SerializedProperty serializedProperty in serializedPropertiesArray)
            {
                if (serializedProperty.name.Contains("_"))
                {
                    totalCount++;
                    if (serializedProperty.boolValue == true)
                    {
                        count++;
                        //Debug.Log(" : " +  serializedProperty.name);
                    }
                }              
            }
            //EditorGUILayout.LabelField("Active: " + count + " / " + serializedPropertiesArray.Length, GUILayout.Width(85));
            //EditorGUILayout.LabelField("(" + count + " / " + serializedPropertiesArray.Length + ")", GUILayout.Width(49));
            int width = CalculateWidthFromPropertyLengthAndEnabled(count, serializedPropertiesArray);
            int serializedPropertiesArrayLength = serializedPropertiesArray.Length;
            //if (serializedPropertiesArrayLength>=100)
            //{
            //    if (count >= 10)
            //    {
            //        width = 53;

            //    }
            //    else
            //    {
            //        width = 45;

            //    }

            //} 
            //else if (serializedPropertiesArrayLength >= 10 && serializedPropertiesArrayLength < 100)
            //{
            //    width = 40;
            //}
            //else
            //{
            //    width = 30;
            //}




            //EditorGUILayout.LabelField(count + " / " + totalCount, GUILayout.Width(width));


            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.richText = true;
            //labelStyle.normal.textColor = myStyleColor;

            EditorGUILayout.LabelField(count + " / " + "<color=grey>" + totalCount + "</color>", labelStyle, GUILayout.Width(width));




            bool toggleTMP = EditorGUILayout.Toggle(toggle.boolValue, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();
            style.fontStyle = previousStyle;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();

            //EditorGUILayout.Space();
            EditorGUILayout.Space(3f);
            if (EditorGUIUtility.isProSkin)
            {
                //EditorUtils.DrawUILineBottom(rect2, new Color(0.25f, 0.25f, 0.25f, 1f));
                Color lightBlue = new Color(1.1f, 1.2f, 1.7f, 0.5f);
                EditorUtils.DrawUILineBottom(rect2, lightBlue, 1);
            }
            else
            {
                EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            }

            EditorGUILayout.EndVertical();


            if (toggle.boolValue != toggleTMP && toggleTMP)
            {
                foldoutBool.boolValue = true;

                SetSerializedPropertiesBool(true, serializedPropertiesArray);


            }
            toggle.boolValue = toggleTMP;

            return (foldoutBool.boolValue && toggle.boolValue);
        }



        public static bool MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(GUIContent guiContent, SerializedProperty foldoutBool, SerializedProperty toggle, Color color, FontStyle fontStyle = FontStyle.Normal,  int plusTextSize = 0, int padding = 0, params SerializedProperty[] serializedProperties)
        {
            if (toggle.boolValue == false)
            {
                foldoutBool.boolValue = false;
                SetSerializedPropertiesBool(false, serializedProperties);

            }

            int count = 0;
            int totalCount = 0;
            foreach (SerializedProperty serializedProperty in serializedProperties)
            {
                if (serializedProperty.name.Contains("_"))
                {
                    //if(serializedProperty.name.Equals("_NumberOfCells"))
                    //{
                    //    Debug.Log("inside foldout" + serializedProperty.boolValue);
                    //}
                    totalCount++;
                    if (serializedProperty.boolValue == true)
                    {
                        count++;
                        //Debug.Log(" : " +  serializedProperty.name);
                    }
                }

            }


            Rect rect2 = EditorGUILayout.BeginVertical();
            rect2.x += padding;
            int tempIndentOri = EditorGUI.indentLevel;
            EditorGUI.indentLevel += padding/10;
            if (count == 0)
            {
                DrawBox(rect2, new Color(1.5f,0,0));
            }
            else
            {
                DrawBox(rect2, color);
            }

            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;

            style.fontStyle = fontStyle;

            style.fontSize = EditorStyles.foldout.fontSize + 1 + plusTextSize;
            Color myStyleColor = new Color(1f, 1f, 1f, 1f);
            Color hoverColor = new Color(0f, 1f, 0.5f, 1f);
            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;

            EditorGUILayout.BeginHorizontal();
            foldoutBool.boolValue = EditorGUILayout.Foldout(foldoutBool.boolValue, guiContent, true, style);
            ////GUILayout.FlexibleSpace();
            ////var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };
            int tempIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            ////string spaceString = "    ";
            //string spaceString = "    ";
            //if (count > 9 )
            //{
            //    spaceString = "";
            //}
            //EditorGUILayout.LabelField(spaceString + count + " / " + totalCount, GUILayout.Width(50));

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.richText = true;
            //labelStyle.normal.textColor = myStyleColor;

            //int width = 0;
            //int serializedPropertiesArrayLength = serializedProperties.Length;
            //if (serializedPropertiesArrayLength >= 100)
            //{
            //    width = 53;
            //}
            //else if (serializedPropertiesArrayLength >= 10 && serializedPropertiesArrayLength < 100)
            //{
            //    width = 43;
            //}
            //else
            //{
            //    width = 33;
            //}
            int width = CalculateWidthFromPropertyLengthAndEnabled(count, serializedProperties);


            EditorGUILayout.LabelField(count + " / " + "<color=grey>" + totalCount + "</color>", labelStyle, GUILayout.Width(width));
            //EditorGUI.indentLevel = tempIndent;
            bool toggleTMP = EditorGUILayout.Toggle(toggle.boolValue, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel = tempIndent;

            style.fontStyle = previousStyle;
            style = EditorStyles.foldout;

            //}
            EditorUtils.DrawUILineBottom(rect2, new Color(0.7f, 0.7f, 0.7f, 1f), 1, 0);
            EditorGUILayout.EndVertical();


            //foldoutBool.value = foldoutBool.value;


            if (toggle.boolValue != toggleTMP && toggleTMP)
            {
                foldoutBool.boolValue = true;
                SetSerializedPropertiesBool(true, serializedProperties);

            }
            toggle.boolValue = toggleTMP;

            EditorGUI.indentLevel = tempIndentOri;

            return (foldoutBool.boolValue && toggle.boolValue);


        }



        public static Tuple<bool, bool> MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(GUIContent guiContent, bool showContent, bool toggle, Color color, int plusTextSize = 0)
        {

            //showDescription = EditorGUILayout.Foldout(showDescription, name, EditorStyles.boldLabel); //, EditorStyles.boldLabel);

            Rect rect2 = EditorGUILayout.BeginVertical();
            //EditorGUILayout.Space(1f);
            rect2.x -= 15;
            rect2.width += 15;
            DrawBox(rect2, color);
            //if (EditorGUIUtility.isProSkin)
            //{
            //    Color lightBlue = new Color(0.5f, 0.6f, 0.7f, 1);
            //    //EditorUtils.DrawUILine(new Color(0.2f, 0.2f, 0.2f, 1f));
            //    EditorUtils.DrawUILine(lightBlue, 1, 0);

            //}
            //else
            //{
            //    Color lightBlue = new Color(0.6f, 0.8f, 2f, 1);
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //    EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            //}

            ////EditorGUILayout.Space();
            //if (EditorGUIUtility.isProSkin)
            //{
            //    //GUI.Box(rect2, GUIContent.none);
            //DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1f));
            //}
            //else
            //{

            //   DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1f));
            //    //DrawBox(rect2, new Color(0.8f, 0.8f, 0.8f, 1));
            //}
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = EditorStyles.foldout.fontSize + 1 + plusTextSize;
            Color myStyleColor = new Color(1f, 1f, 1f, 1f);
            Color hoverColor = new Color(0f, 1f, 0.5f, 1f);
            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;

            EditorGUILayout.BeginHorizontal();
            showContent = EditorGUILayout.Foldout(showContent, guiContent, true, style); //, EditorStyles.boldLabel);
                                                                                         //GUILayout.FlexibleSpace();
                                                                                         //enableShadingStyling.floatValue = Convert.ToSingle(EditorGUILayout.Toggle(Convert.ToBoolean(enableShadingStyling.floatValue), GUILayout.Width(15)));
            toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();

            //showContent = EditorGUILayout.Foldout(showContent, guiContent, style); //, EditorStyles.boldLabel);



            style.fontStyle = previousStyle;
            style = EditorStyles.foldout;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();



            //if (EditorGUIUtility.isProSkin)
            //{
            //    EditorUtils.DrawUILine(new Color(0.25f, 0.25f, 0.25f, 1f));
            //}
            //else
            //{
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //}
            EditorUtils.DrawUILineBottom(rect2, new Color(0.7f, 0.7f, 0.7f, 1f), 1, 0);
            EditorGUILayout.EndVertical();

            return Tuple.Create(showContent, toggle);
        }
        public static Tuple<bool, bool> MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(GUIContent guiContent, bool showContent, bool toggle, int plusTextSize = 0)
        {

            //showDescription = EditorGUILayout.Foldout(showDescription, name, EditorStyles.boldLabel); //, EditorStyles.boldLabel);

            Rect rect2 = EditorGUILayout.BeginVertical();

            //if (EditorGUIUtility.isProSkin)
            //{
            //    Color lightBlue = new Color(0.5f, 0.6f, 0.7f, 1);
            //    //EditorUtils.DrawUILine(new Color(0.2f, 0.2f, 0.2f, 1f));
            //    EditorUtils.DrawUILine(lightBlue, 1, 0);

            //}
            //else
            //{
            //    Color lightBlue = new Color(0.6f, 0.8f, 2f, 1);
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //    EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            //}

            ////EditorGUILayout.Space();
            //if (EditorGUIUtility.isProSkin)
            //{
            //    //GUI.Box(rect2, GUIContent.none);
            //DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1f));
            //}
            //else
            //{

            //   DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1f));
            //    //DrawBox(rect2, new Color(0.8f, 0.8f, 0.8f, 1));
            //}
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = EditorStyles.foldout.fontSize + 1 + plusTextSize;
            Color myStyleColor = new Color(1f, 1f, 1f, 1f);
            Color hoverColor = new Color(0f, 1f, 0.5f, 1f);
            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;

            EditorGUILayout.BeginHorizontal();
            showContent = EditorGUILayout.Foldout(showContent, guiContent, true, style); //, EditorStyles.boldLabel);
                                                                                         //GUILayout.FlexibleSpace();
                                                                                         //enableShadingStyling.floatValue = Convert.ToSingle(EditorGUILayout.Toggle(Convert.ToBoolean(enableShadingStyling.floatValue), GUILayout.Width(15)));
            toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();

            //showContent = EditorGUILayout.Foldout(showContent, guiContent, style); //, EditorStyles.boldLabel);



            style.fontStyle = previousStyle;
            style = EditorStyles.foldout;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();



            //if (EditorGUIUtility.isProSkin)
            //{
            //    EditorUtils.DrawUILine(new Color(0.25f, 0.25f, 0.25f, 1f));
            //}
            //else
            //{
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //}

            EditorGUILayout.EndVertical();

            return Tuple.Create(showContent, toggle);
        }

        public static bool MakeTheToonShaderSubSubSectionHeaderWithFoldout(GUIContent guiContent, bool showContent)
        {
            EditorGUI.indentLevel += 1;
            //showDescription = EditorGUILayout.Foldout(showDescription, name, EditorStyles.boldLabel); //, EditorStyles.boldLabel);

            Rect rect2 = EditorGUILayout.BeginVertical();

            //if (EditorGUIUtility.isProSkin)
            //{
            //    Color lightBlue = new Color(0.5f, 0.6f, 0.7f, 1);
            //    //EditorUtils.DrawUILine(new Color(0.2f, 0.2f, 0.2f, 1f));
            //    EditorUtils.DrawUILine(lightBlue, 1, 0);

            //}
            //else
            //{
            //    Color lightBlue = new Color(0.6f, 0.8f, 2f, 1);
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //    EditorUtils.DrawUILine(rect2, lightBlue, 1, -5);
            //}

            ////EditorGUILayout.Space();
            //if (EditorGUIUtility.isProSkin)
            //{
            //    //GUI.Box(rect2, GUIContent.none);
            //DrawBox(rect2, new Color(0.5f, 1.5f, 2.5f, 1f));
            //}
            //else
            //{

            //   DrawBox(rect2, new Color(0.5f, 0.7f, 0.9f, 1f));
            //    //DrawBox(rect2, new Color(0.8f, 0.8f, 0.8f, 1));
            //}
            GUIStyle style = new GUIStyle(EditorStyles.foldout);
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = EditorStyles.foldout.fontSize + 1;
            Color myStyleColor = new Color(1f, 1f, 1f, 1f);
            Color hoverColor = new Color(0f, 1f, 0.5f, 1f);
            style.normal.textColor = myStyleColor;
            style.onNormal.textColor = myStyleColor;
            style.hover.textColor = myStyleColor;
            style.onHover.textColor = myStyleColor;
            style.focused.textColor = myStyleColor;
            style.onFocused.textColor = myStyleColor;
            style.active.textColor = myStyleColor;
            style.onActive.textColor = myStyleColor;
            showContent = EditorGUILayout.Foldout(showContent, guiContent, style); //, EditorStyles.boldLabel);
            style.fontStyle = previousStyle;
            style = EditorStyles.foldout;
            //GUILayout.Label(name, EditorStyles.boldLabel);

            //EditorGUILayout.Space();



            //if (EditorGUIUtility.isProSkin)
            //{
            //    EditorUtils.DrawUILine(new Color(0.25f, 0.25f, 0.25f, 1f));
            //}
            //else
            //{
            //    EditorUtils.DrawUILine(rect2, new Color(0.1f, 0.1f, 0.1f, 1f), 2, -8);
            //}

            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel -= 1;
            return showContent;
        }

        public static Color ShadingColor = new Color(0, 0, 0);

        public static void SetSerializedPropertiesBool(bool boolValue, params SerializedProperty[] serializedProperties)
        {
            foreach (SerializedProperty serializedProperty in serializedProperties)
                serializedProperty.boolValue = boolValue;
        }

    }
}