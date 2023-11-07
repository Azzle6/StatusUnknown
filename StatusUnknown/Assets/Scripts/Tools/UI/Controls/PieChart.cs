using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

public class PieChart : BindableElement, INotifyValueChanged<float>
{
    //////////////////////////////////////////////////////////////////////////
    // UxmlFactories
    /////////////////////////////////////////////////////////////////////////
    
    public new class UxmlFactory : UxmlFactory<PieChart, UxmlTraits> { }
    public new class UxmlTraits : BindableElement.UxmlTraits
    {
        UxmlFloatAttributeDescription _Radius = new UxmlFloatAttributeDescription { name = "radius", defaultValue = 60.0f };
        UxmlFloatAttributeDescription _Value = new UxmlFloatAttributeDescription { name = "value", defaultValue = 10.0f };
        UxmlFloatAttributeDescription _LowValue = new UxmlFloatAttributeDescription { name = "low-value", defaultValue = 0.0f };
        UxmlFloatAttributeDescription _HighValue = new UxmlFloatAttributeDescription { name = "high-value", defaultValue = 100.0f };
        UxmlColorAttributeDescription _FirstColor = new UxmlColorAttributeDescription { name = "first-color", defaultValue = new Color32(180, 240, 120, 255) };
        UxmlColorAttributeDescription _SecondColor = new UxmlColorAttributeDescription { name = "second-color", defaultValue = new Color32(250, 120, 20, 255) };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var pie = ve as PieChart;
            pie.radius = _Radius.GetValueFromBag(bag, cc);
            pie.lowValue = _LowValue.GetValueFromBag(bag, cc);
            pie.highValue = _HighValue.GetValueFromBag(bag, cc);
            pie.firstColor = _FirstColor.GetValueFromBag(bag, cc);
            pie.secondColor = _SecondColor.GetValueFromBag(bag, cc);
            pie.value = _Value.GetValueFromBag(bag, cc);
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // Member Variables
    /////////////////////////////////////////////////////////////////////////

    float _Radius = 60.0f;
    float _Diameter = 120.0f;
    float _LowValue = 0.0f;
    float _HighValue = 100f;
    float _Value = 10f;
    Color _FirstColor = new Color32(180, 240, 120, 255);
    Color _SecondColor = new Color32(250, 120, 20, 255);
    VisualElement _Chart;

    //////////////////////////////////////////////////////////////////////////
    // Public Properties
    /////////////////////////////////////////////////////////////////////////

    public float radius { get => _Radius; set => _Radius = value; }  
    public float diameter => _Diameter;
    public float lowValue { get => _LowValue; set => _LowValue = value; }
    public float highValue { get => _HighValue; set => _HighValue = value; }
    public Color firstColor { get => _FirstColor; set => _FirstColor = value; }
    public Color secondColor { get => _SecondColor; set => _SecondColor = value; }

    //////////////////////////////////////////////////////////////////////////
    // Value Property
    /////////////////////////////////////////////////////////////////////////

    public float value { get => _Value; set => _Value = value; }

    //////////////////////////////////////////////////////////////////////////
    // Constructor
    /////////////////////////////////////////////////////////////////////////

    public PieChart()
    {
        style.alignItems = Align.Center;

        _Chart = new VisualElement();
        _Chart.style.height = _Diameter;
        _Chart.style.width = _Diameter;
        _Chart.generateVisualContent += DrawCanvas;

        Add(_Chart); 
    }

    //////////////////////////////////////////////////////////////////////////
    // INotifyValueChanged
    /////////////////////////////////////////////////////////////////////////

    public void SetValueWithoutNotify(float newValue)
    {
        _Value = newValue; 
        _Chart.MarkDirtyRepaint();
    }

    //////////////////////////////////////////////////////////////////////////
    // Canvas Drawing
    /////////////////////////////////////////////////////////////////////////

    private void DrawCanvas(MeshGenerationContext ctx)
    {
        var percentage = ((_Value - lowValue) / (highValue - lowValue)) * 100;

        var painter = ctx.painter2D;
        painter.strokeColor = Color.white; 
        painter.fillColor = Color.white;

        var percentages = new float[]
        {
            percentage,
            100 - percentage
        };
        var colors = new Color32[]
        {
            _FirstColor,
            _SecondColor
        };

        float angle = 0.0f;
        float anglePct = 0.0f;
        int k = 0;
        foreach (var pct in percentages)
        {
            anglePct += 360.0f * (pct / 100); 

            painter.fillColor = colors[k++];
            painter.BeginPath(); 
            painter.MoveTo(new Vector2(_Radius, _Radius));  
            painter.Arc(new Vector2(_Radius, _Radius), _Radius, angle, anglePct);
            painter.Fill();

            angle = anglePct; 
        }
    }
}
