using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class WireVisual : MonoBehaviour
    {
        LineRenderer[] wires;

        [SerializeField]
        private Transform[] connections = null;

        void Awake()
        {
            wires = GetComponentsInChildren<LineRenderer>();

            Color line_color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            for (int i = 0; i < wires.Length; ++i)
            {
                wires[i].SetPosition(0, transform.parent.position);
                wires[i].startColor = wires[i].endColor = line_color;
            }

            do
            {
                Lever lever = transform.parent.GetComponent<Lever>();
                if (lever != null)
                {
                    connections = new Transform[1];
                    connections[0] = lever.triggerObject.transform;
                    break;
                }

                LeverMultiTrigger lever_multi = transform.parent.GetComponent<LeverMultiTrigger>();
                if (lever_multi != null)
                {
                    connections = new Transform[lever_multi.triggerObjects.Count];
                    for (int i = 0; i < lever_multi.triggerObjects.Count; ++i)
                        connections[i] = lever_multi.triggerObjects[i].transform;
                    break;
                }

                ButtonLevel1 button = transform.parent.GetComponent<ButtonLevel1>();
                if (button != null)
                {
                    connections = new Transform[1];
                    connections[0] = button.spike.transform;
                    break;
                }

                if (connections == null)
                    connections = new Transform[0];
            } while (false);

            for (int i = 0; i < connections.Length; ++i)
                wires[i].SetPosition(1, connections[i].position);

            for (int i = connections.Length; i < wires.Length; ++i)
                wires[i].gameObject.SetActive(false);
        }

        void Update()
        {
            for (int i = 0; i < connections.Length; ++i)
                wires[i].SetPosition(1, connections[i].position);
        }
    }
}
