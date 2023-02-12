using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.VertexColorPaletteChanger
{
    public class VertexColorPaletteChangerBehaviour : MonoBehaviour
    {
        public Color32[] Palette;
        public bool IgnoreAlpha = true;
        
        [ContextMenu(nameof(SetColors))]
        public void SetColors()
        {
            MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>(true);
            var meshes = new HashSet<Mesh>(meshFilters.Select(x => x.sharedMesh));
            foreach (var mesh in meshes) {
                if (mesh == null) {
                    continue;
                }
                
                mesh.colors32 = ManipulateColorArray(mesh.colors32);
            }
        }

        private Color32[] ManipulateColorArray(Color32[] colors)
        {
            for (var i = 0; i < colors.Length; i++) {
                colors[i] = GetClosestPaletteColor(colors[i]);
            }

            return colors;
        }

        private Color32 GetClosestPaletteColor(Color32 input)
        {
            if (Palette.Length == 0) {
                return new Color32(0, 0, 0, 0);
            }
            
            var closestIndex = 0;
            for (var i = 1; i < Palette.Length; i++) {
                if (ColorDiff(Palette[i], input) < ColorDiff(Palette[closestIndex], input)) {
                    closestIndex = i;
                }
            }

            return IgnoreAlpha
                ? new Color32(Palette[closestIndex].r, Palette[closestIndex].g, Palette[closestIndex].b, input.a)
                : Palette[closestIndex];
        }

        // distance in RGB space
        private static int ColorDiff(Color32 c1, Color32 c2) 
        {
            return (int) Math.Sqrt((c1.r - c2.r) * (c1.r - c2.r) 
                                   + (c1.g - c2.g) * (c1.g - c2.g)
                                   + (c1.b - c2.b) * (c1.b - c2.b));
        }
    }
}
