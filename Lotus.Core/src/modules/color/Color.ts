/* eslint-disable prefer-rest-params */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { ColorHelper } from './ColorHelper';
import { IColorModelHSL } from './ColorModel';
import { ColorNames } from './ColorNames';

/** @class Color
* Color class accepts a CSS color string, rgb, hsl data as the input, manipulate the color, and returns a CSS-compatible color string.
* @constructor
*
* @example
* new Color('red')  // named CSS colors
*
* @example
* new Color('red', 0.5)  // named CSS colors and transparency
*
* @example
* new Color('#f00')  // hex 3 characters
*
* @example
* new Color('#e2b644')  // hex 6 characters
*
* @example
* new Color('rgb(255, 0, 100)')  // rgb()
*
* @example
* new Color('rgba(255, 0, 100, 0.5)')  // rgba()
*
* @example
* new Color('rgba(255, 0, 100, 0.5)', 0.1)  // 0.1 overrides alpha from rgba
*
* @example
* new Color([255,0,0])  // rgb array
*
* @example
* new Color([255,0,0], 0.5)  // rgb and transparency
*
* @example
* new Color({  // hsl object
*     h: 0.2,
*     s: 0.5,
*     l: 1
* })
*
* @example
* new Color({  // hsl object and transparency
*     h: 0.5,
*     s: 1,
*     l: 1
* }, 0.5)
*/
export class Color 
{
  private rgb: number[] | undefined;
  private hsl: IColorModelHSL | undefined;
  private a: number;

  constructor();
  constructor(any: any);
  constructor(any: Color);
  constructor(any: string);
  constructor(any: number[]);
  constructor(any: IColorModelHSL);
  constructor(any: Color, alpha: number);
  constructor(any: string, alpha: number);
  constructor(any: number[], alpha: number);
  constructor(any: IColorModelHSL, alpha: number);
  constructor(red: number, green: number, blue: number);
  constructor(red: number, green: number, blue: number, alpha: number);
  constructor(red?: any, green?: number, blue?: number, alpha?: number) 
  {
    if (arguments.length === 0) 
    {
      this.rgb = [0, 0, 0];
      this.a = 0;
    }

    else if (typeof arguments[0] === 'number') 
    {
      if (arguments.length === 3 && ColorHelper.isRGBArray([red, green, blue])) 
      {
        this.rgb = [red, green, blue];
        this.a = 1;
      }
      else if (arguments.length === 4 && ColorHelper.isRGBAArray([red, green, blue, alpha])) 
      {
        this.rgb = [red, green, blue];
        this.a = alpha!;
      }
      else throw Error('invalid color');
    }
    else if (typeof arguments[0] === 'string') 
    {
      const rgba = ColorHelper.parseColorString(arguments[0]);
      if (rgba) 
      {
        this.rgb = rgba.slice(0, 3);
        if (arguments.length === 2 && ColorHelper.isAlphaValue(arguments[1])) 
        {
          this.a = arguments[1];
        }
        else if (rgba.length === 4) 
        {
          this.a = rgba[3];
        }
        else 
        {
          this.a = 1;
        }
      }
      else throw Error('invalid color');
    }
    else if (typeof arguments[0] === 'object') 
    {
      const obj = arguments[0];
      if (obj.length > 0) 
      {
        if (obj.length === 3 && ColorHelper.isRGBArray(obj)) 
        {
          this.rgb = obj.slice(0, 3);
          if (arguments.length === 2) 
          {
            if (ColorHelper.isAlphaValue(arguments[1])) 
            {
              this.a = arguments[1];
            }
            else throw new Error('invalid alpha value');
          }
          else 
          {
            this.a = 1;
          }
        }
        else throw Error('invalid color');
      }
      else 
      {
        if (obj instanceof Color) 
        {
          if (obj.hsl) 
          {
            this.hsl = {
              h: obj.hsl.h,
              s: obj.hsl.s,
              l: obj.hsl.l
            };
          }
          if (obj.rgb) 
          {
            this.rgb = obj.rgb.slice();
          }
          if (arguments.length === 2) 
          {
            if (ColorHelper.isAlphaValue(arguments[1])) 
            {
              this.a = arguments[1];
            }
            else throw new Error('invalid alpha value');
          }
          else 
          {
            this.a = obj.a;
          }
        }
        else if (ColorHelper.isHSL(obj)) 
        {
          this.hsl = {
            h: obj.h,
            s: obj.s,
            l: obj.l
          };
          if (arguments.length === 2) 
          {
            if (ColorHelper.isAlphaValue(arguments[1])) 
            {
              this.a = arguments[1];
            }
            else throw new Error('invalid alpha value');
          }
          else 
          {
            this.a = 1;
          }
        }
        else throw Error('invalid color');
      }
    }
    else throw new Error('invalid color');
  }

  private _getRGB(): number[] 
  {
    if (!this.rgb) 
    {
      this.rgb = ColorHelper.hsl2rgb(this.hsl);
    }
    return this.rgb;
  }

  /**
   * Return the red, green, blue color values with the alpha channel as an array
   *
   * @method getRGB
   * @memberof Color
   * @return {Array} rgba the array of color values
   * @instance
   *
   * @example
   * new Color('red).getRGB();   // returns [255,0,0]
   *
   */
  getRGB(): number[] 
  {
    return this._getRGB().slice();
  }

  /**
   * Returns the hexadecimal value of the color
   *
   * @method getHex
   * @memberof Color
   * @return {String} hex color value
   * @instance
   *
   * @example
   * new Color('rgba(255,0,0,0.5)').getHex(); // returns "#f00"
   *
   */
  getHex(): string 
  {
    return ColorHelper.rgb2hex(this._getRGB());
  }

  private _getHSL(): IColorModelHSL 
  {
    if (!this.hsl) 
    {
      this.hsl = ColorHelper.rgb2hsl(this.rgb!);
    }
    return this.hsl;
  }

  /**
   * Returns an [h,s,l] array from color string
   *
   * @method getHSL
   * @memberof Color
   * @return {Number[]} hsl array of [hue,saturation,lightness]
   * @instance
   *
   * @example
   * new Color('#f00').getHSL(); // returns [0,1,0.5]
   *
   */
  getHSL(): IColorModelHSL 
  {
    const hsl = this._getHSL();
    return {
      h: hsl.h,
      s: hsl.s,
      l: hsl.l
    }
  }

  /**
   * Sets the transparency of a color
   *
   * @method alpha
   * @memberof Color
   * @param {Number} alpha transparency level between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('#f00').alpha(0.5).toString();  // returns "rgba(255,0,0,0.5)"
   *
   */
  alpha(alpha: number): Color 
  {
    if (ColorHelper.isAlphaValue(alpha)) 
    {
      if (this.hsl) 
      {
        return new Color(this.getHSL(), alpha);
      }
      else 
      {
        return new Color(this.getRGB(), alpha);
      }
    }
    else 
    {
      throw new Error('invalid alpha value');
    }
  }

  /**
   * Returns the red component of a color string
   *
   * @method getRed
   * @memberof Color
   * @return {Number} red component 0-255
   * @instance
   *
   * @example
   * new Color('#fff').getRed(); // returns 255
   *
   */
  getRed(): number 
  {
    return this._getRGB()[0];
  }

  /**
   * Set the red component of a color
   *
   * @method red
   * @memberof Color
   * @param {Number} red red component 0-255
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('rgb(0,0,255)').red(255).toString();  // returns "#F0F"
   *
   */
  red(r: number): Color 
  {
    if (ColorHelper.isColorValue(r)) 
    {
      const rgb = this._getRGB();
      return new Color( [r, rgb[1], rgb[2]], this.a);
    }
    else throw new Error('invalid red');
  }

  /**
   * Returns the green component of a color string
   *
   * @method getGreen
   * @memberof Color
   * @return {Number} green component 0-255
   * @instance
   *
   * @example
   * new Color('#fff').getGreen(); // returns 255
   *
   */
  getGreen():number 
  {
    return this._getRGB()[1];
  }

  /**
   * Set the green component of a color
   *
   * @method green
   * @memberof Color
   * @param {Number} green green component 0-255
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('rgb(255,0,0)').green(255).toString();  // returns "#FF0"
   *
   */
  green(g: number): Color 
  {
    if (ColorHelper.isColorValue(g)) 
    {
      const rgb = this._getRGB();
      return new Color( [rgb[0], g, rgb[2]], this.a);
    }
    else throw new Error('invalid green');
  }

  /**
   * Returns the blue component of a color string
   *
   * @method getBlue
   * @memberof Color
   * @return {Number} blue component 0-255
   * @instance
   *
   * @example
   * new Color('#fff').getBlue(); // returns 255
   *
   */
  getBlue(): number 
  {
    return this._getRGB()[2];
  }

  /**
   * Set the blue component of a color
   *
   * @method blue
   * @memberof Color
   * @param {Number} blue blue component 0-255
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('#FF0').blue(255).toString();  // returns "#FFF"
   *
   */
  blue(b: number): Color 
  {
    if (ColorHelper.isColorValue(b)) 
    {
      const rgb = this._getRGB();
      return new Color( [rgb[0], rgb[1], b], this.a);
    }
    else throw new Error('invalid blue');
  }

  /**
   * Returns the transparency of a color
   *
   * @method getAlpha
   * @memberof Color
   * @return {Number} alpha transparency level between 0 and 1
   * @instance
   *
   * @example
   * new Color('#F00').getAlpha(); // returns 0
   * new Color('rgba(255,0,0,0.5)').getAlpha(); // returns 0.5
   *
   */
  getAlpha(): number 
  {
    return this.a;
  }

  /**
   * Return the "saturation" of a color
   *
   * @method getSaturation
   * @memberof Color
   * @return {Number} saturation saturation value between 0 and 1
   * @instance
   v
   * @example
   * new Color('rgb(100,100,100)').getSaturation(); // returns 0
   * new Color('rgb(100,50,100)').getSaturation();  // returns 0.8333333333333334
   * new Color('rgb(100,0,100)').getSaturation();   // returns 1
   *
   */
  getSaturation(): number 
  {
    const hsl = this._getHSL();
    return hsl.s;
  }

  /**
   * Return the "hue" of a color
   *
   * @method getHue
   * @memberof Color
   * @return {Number} hue hue value between 0 and 1
   * @instance
   *
   * @example
   * new Color('#a1b2c1').getHue(); // returns "0.578125"}
   * new Color('#f00').getHue(); // returns 0
   * new Color('#0f0').getHue(); // returns 0.3333333333333333
   * new Color('#00f').getHue(); // returns 0.6666666666666666
   *
   */
  getHue(): number 
  {
    const hsl = this._getHSL();
    return hsl.h;
  }

  /**
   * Set the "hue" of a color
   *
   * @method hue
   * @memberof Color
   * @param {Number} hue hue value between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('#f00').hue(2/3).toString(); // returns "#00f"
   * new Color('#0f0').hue(1/3).toString(); // returns "#0f0"
   * new Color('#00f').hue(0.23).toString(); // returns "#9eff00"
   *
   */
  hue(hue: number): Color 
  {
    if (ColorHelper.isAlphaValue(hue)) 
    {
      const hsl = this._getHSL();
      return new Color( {
        h: hue,
        s: hsl.s,
        l: hsl.l
      }, this.a);
    }
    else throw new Error('invalid hue');
  }

  /**
   * Shifts the "hue" of a color value by a given percentage
   *
   * @method shiftHue
   * @memberof Color
   * @param {Number} hueShift amount to modify the hue by between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color(255,255,0).shiftHue(0.25).toString(); // returns "#00ff7f"
   *
   */
  shiftHue(amount: number): Color 
  {
    const hsl = this._getHSL();
    let newHue = hsl.h + amount;
    if (newHue > 1) 
    {
      const x = Math.floor(newHue);
      newHue -= x;
    }
    if (newHue < -1) 
    {
      const x = Math.floor(newHue);
      newHue += Math.abs(x);
    }
    if (newHue < 0) 
    {
      newHue += 1;
    }

    return new Color( {
      h: newHue,
      s: hsl.s,
      l: hsl.l
    }, this.a);
  }

  /**
   * Set the "saturation" of a color
   *
   * @method saturation
   * @memberof Color
   * @param {Number} saturation saturation value between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color(100,50,50).saturation(0.5).toString().toBe("#712626");
   *
   */
  saturation(saturation: number): Color 
  {
    if (ColorHelper.isAlphaValue(saturation)) 
    {
      const hsl = this._getHSL();
      return new Color( {
        h: hsl.h,
        s: saturation,
        l: hsl.l
      }, this.a);
    }
    else throw new Error('invalid saturation');
  }

  /**
   * Increases the "saturation" of a color value
   *
   * @method saturate
   * @memberof Color
   * @param {Number} saturateBy amount to saturate between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('corn silk 3').saturate(0.1).toString(); // returns "#d3ccab"
   *
   */
  saturate(amount: number): Color 
  {
    if (amount >= -1 && amount <= 1) 
    {
      let s = this.getSaturation();
      s += amount;
      if (s > 1) s = 1;
      if (s < 0) s = 0;
      return this.saturation(s);
    }
    else throw new Error('invalid saturate');
  }

  /**
   * Decreases the "saturation" of a color value
   *
   * @method desaturate
   * @memberof Color
   * @param {Number} desaturateBy amount to desaturate between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('#d3ccab').desaturate(0.1).toString(); // returns "#cdc8b1"
   *
   */
  desaturate(amount: number): Color 
  {
    return this.saturate(-amount);
  }

  /**
   * Return the lightness of a color (how close to white or black the color is)
   *
   * @method getLightness
   * @memberof Color
   * @return {Number} lightness lightness value between 0 and 1
   * @instance
   *
   * @example
   * new Color('rgb(0,0,0)').getLightness();       // returns 0
   * new Color('rgb(100,50,100)').getLightness();  // returns 0.29411764705882354
   * new Color('rgb(255,255,255)').getLightness(); // returns 1
   *
   */
  getLightness(): number 
  {
    const hsl = this._getHSL();
    return hsl.l;
  }

  /**
   * Set the lightness of a color, how close to white or black the color will be
   *
   * @method lightness
   * @memberof Color
   * @param {Number} lightness lightness value between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('rgb(255,0,0)').lightness(0).toString(); // returns "#000"
   * new Color('rgb(255,0,0)').lightness(0.5).toString(); // returns "#F00"
   * new Color('rgb(255,0,0)').lightness(1).toString(); // returns "#FFF"
   *
   */
  lightness(lightness: number): Color 
  {
    if (ColorHelper.isAlphaValue(lightness)) 
    {
      const hsl = this._getHSL();
      return new Color( {
        h: hsl.h,
        s: hsl.s,
        l: lightness
      }, this.a);
    }
    else
    {
      return ColorNames.white;
    }
  }

  /**
   * Increases the "lightness" of a color value
   *
   * @method lighten
   * @memberof Color
   * @param {Number} lightenBy amount to lighten between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('#f00').lighten(0.5).toString(); // returns "#FF8080"
   *
   */
  lighten(amount: number): Color 
  {
    if (amount >= -1 && amount <= 1) 
    {
      const hsl = this._getHSL();
      let l = hsl.l + amount;
      if (l > 1) l = 1;
      if (l < 0) l = 0;
      return new Color( {
        h: hsl.h,
        s: hsl.s,
        l
      }, this.a);
    }
    else throw new Error('invalid lighten');
  }

  /**
   * Decreases the "lightness" of a color value
   *
   * @method darken
   * @memberof Color
   * @param {Number} darkenBy amount to darken between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('#f00').darken(0.5).toString(); // returns "#800000"
   *
   */
  darken(amount: number): Color 
  {
    return this.lighten(-amount);
  }

  /**
   * Changes the color closer to another color by a given percentage
   *
   * @method combine
   * @memberof Color
   * @param {Object} targetColor color string, array, or object
   * @param {Number} [amount=0.5] how close to the target color between 0 and 1 (0.5 is half-way between)
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('black').combine('red', 0.5).toString(); // returns "#800000"
   *
   */
  combine(colorValue: any, percentage: number ): Color 
  {
    if (ColorHelper.isAlphaValue(percentage)) 
    {
      let color;
      if (colorValue instanceof Color) 
      {
        color = colorValue;
      }
      else 
      {
        color = new Color(colorValue);
      }
      const newRgb = ColorHelper.combine(this._getRGB(), color._getRGB(), percentage);
      return new Color(newRgb, this.a);
    }
    else throw new Error('invalid combine percentage');
  }

  /**
   * Inverts the color
   *
   * @method invert
   * @memberof Color
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('#f00').invert(1).toString(); // returns "#0FF"
   * new Color('#fff').invert().toString();  // returns "#000"
   *
   */
  invert(): Color 
  {
    return new Color(ColorHelper.invert(this._getRGB()), this.a );
  }

  /**
   * Shifts only the hue of a color closer to another color by a given percentage
   *
   * @method tint
   * @memberof Color
   * @param {String} targetColor color string or array
   * @param {Number} amount amount to shift the hue toward the target color between 0 and 1
   * @return {Color} new Color() instance
   * @instance
   *
   * @example
   * new Color('#f00').tint('#00f',0.5).toString(); // returns "#0f0"
   * new Color('rgb(0,0,100)').tint('rgb(100,0,0)',0.1).toString(); // returns "#002864"
   *
   */
  tint (colorValue: any, percentage: number | undefined): Color 
  {
    let color;
    if (colorValue instanceof Color) 
    {
      color = colorValue;
    }
    else 
    {
      color = new Color(colorValue);
    }
    if (typeof percentage === 'undefined') 
    {
      percentage = 0.5;
    }
    const h = ColorHelper.tint(this.getHue(), color.getHue(), percentage);
    return new Color({
      h,
      s: this.hsl!.s,
      l: this.hsl!.l
    }, this.a);
  }

  /**
   * Returns the CSS string of the color, either as hex value, or rgba if an alpha value is defined
   *
   * @method toString
   * @memberof Color
   * @return {String} css color value
   * @instance
   *
   * @example
   * new Color('rgb(0,0,255)').toString(); // returns "#00f"
   *
   */
  toString(): string 
  {
    if (this.a === 0) 
    {
      return 'transparent';
    }
    if (this.a < 1) 
    {
      const rgb = this._getRGB();
      return 'rgba(' + rgb[0] + ',' + rgb[1] + ',' + rgb[2] + ',' + this.a + ')';
    }
    else 
    {
      return this.getHex();
    }
  }

  /**
   * Returns the array of named color values
   *
   * @method getNames
   * @memberof Color
   * @return {Array} named color values
   * @instance
   *
   * @example
   * new Color('#f00').tint('#00f',0.5).toString(); // returns "#0f0"
   * new Color('rgb(0,0,100)').tint('rgb(100,0,0)',0.1).toString(); // returns "#002864"
   *
   */
  static getNames(): any 
  {
    return ColorNames;
  }
}