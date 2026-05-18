using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Slider : Component, ICanvasComponent, ICanvasDrawableComponent
{
    public Sprite Sprite { get => _background; set => _background = value; }
    public Color BackgroundColor { get; set; }
    public Color SliderColor { get; set; }
    public float Value { get; set; }
    public Rectangle Rect { get => _background.Rect; }
    private readonly float _transparent;
    private readonly Sprite _slider;
    private readonly bool _isLerping;
    private readonly IFloatValueProvider _valueProvider;
    private Sprite _background;
    private float _prevValue;
    private Color _fromColor;
    private Color _toColor;


    public Slider(Sprite background, Sprite slider, IFloatValueProvider value = null, float transparent = 1f)
    {
        _background = background;
        _slider = slider;
        _transparent = transparent;
        _valueProvider = value;
        BackgroundColor = Color.White;
        SliderColor = Color.White;
    }

    public Slider(Sprite background, Sprite slider, Color fromColor, Color toColor, IFloatValueProvider value = null, float transparent = 1f)
        : this(background, slider, value, transparent)
    {
        _fromColor = fromColor;
        _toColor = toColor;
        _isLerping = true;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Active & gameObject.Active)
        {
            Vector2 origin = new(_background.Rect.Width / 2, _background.Rect.Height / 2);
            spriteBatch.Draw(_background.Image, new Vector2(gameObject.DrawPosition.X, gameObject.DrawPosition.Y), _background.Rect,
                BackgroundColor * _transparent, gameObject.DrawRotation.Z, origin, new Vector2(gameObject.DrawScale.X,gameObject.DrawScale.Y), SpriteEffects.None, 1);

            float clampValue = Math.Clamp(
                _valueProvider == null
                ? Value
                : _valueProvider.Value, 0f, 1f);

            if (_isLerping)
                LerpColor(clampValue);

            Rectangle newRect = new(Point.Zero, new(Convert.ToInt32(_slider.Rect.Width * clampValue), _slider.Rect.Height));
            Vector2 sliderOrigin = new(_slider.Rect.Width / 2, _slider.Rect.Height / 2);
            spriteBatch.Draw(_slider.Image, new Vector2(gameObject.DrawPosition.X, gameObject.DrawPosition.Y), newRect,
                SliderColor * _transparent, gameObject.DrawRotation.Z, sliderOrigin, new Vector2(gameObject.DrawScale.X,gameObject.DrawScale.Y), SpriteEffects.None, 1);
        }
    }

    private void LerpColor(float value)
    {
        if (_prevValue == value) return;
        
        SliderColor = Color.Lerp(_toColor, _fromColor, _valueProvider.Value);
        _prevValue = _valueProvider.Value;
    }
}