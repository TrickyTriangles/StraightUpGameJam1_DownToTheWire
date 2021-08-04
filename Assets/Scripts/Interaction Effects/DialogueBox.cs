using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox
{
    string text;
    float text_speed;
    AudioClip clip;

    public DialogueBox(string m_text, float m_text_speed, string sfx_name)
    {
        text = m_text;
        text_speed = m_text_speed;

        clip = CommonResources.GetAudioClip(sfx_name);
    }

    public IEnumerator ProcessDialgoueBox()
    {
        GameObject db = GameObject.Instantiate(CommonResources.dialogue_box_prefab, new Vector3(), Quaternion.identity);
        TextMeshProUGUI textbox = db.GetComponentInChildren<TextMeshProUGUI>();

        yield return new WaitForEndOfFrame();

        if (textbox != null)
        {
            int text_len = text.Length;
            bool z_pressed = false;
            bool break_flag = false;
            string display_text = "";

            z_pressed = Input.GetKey(KeyCode.Z);

            for (int i = 0; i < text_len; i++)
            {
                float timer = 0f;

                while (timer < text_speed)
                {
                    timer += Time.unscaledDeltaTime;

                    if (Input.GetKeyDown(KeyCode.Z) && !z_pressed)
                    {
                        i = text_len;
                        break_flag = true;
                        z_pressed = true;
                        yield return null;
                        break;
                    }

                    z_pressed = Input.GetKey(KeyCode.Z);
                    yield return null;
                }

                if (break_flag)
                {
                    display_text = text;
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    display_text += text[i];

                    if (!string.IsNullOrWhiteSpace(text[i].ToString()))
                    {
                        if (clip != null)
                        {
                            CommonResources.PlaySound(clip, Random.Range(0.6f, 0.8f));
                        }
                    }
                }

                textbox.text = display_text;
            }

            if (break_flag)
            {
                while (z_pressed)
                {
                    z_pressed = Input.GetKey(KeyCode.Z);

                    yield return null;
                }
            }

            while (!Input.GetKeyDown(KeyCode.Z))
            {
                yield return null;
            }
        }

        GameObject.Destroy(db);
    }
}