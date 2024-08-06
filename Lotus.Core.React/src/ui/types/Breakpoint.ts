/**
 * Контрольные точки (Breakpoints) - это триггеры настраиваемой ширины
 */
export enum TBreakpoint 
{

  /**
   * Ширина менее 576 пикселей;
   */
  XSmall = 'x-sm',

  /**
   * Ширина не менее 576 пикселей
   */
  Small = 'sm',

  /**
   * Ширина не менее 768 пикселей
   */
  Medium = 'md',

  /**
   * Ширина не менее 992 пикселей
   */
  Large = 'lg',

  /**
   * Ширина не менее 1200 пикселей
   */
  ExtraLarge = 'xl',

  /**
   * Ширина не менее 1400 пикселей
   */
  ExtraExtraLarge = 'xxl',
}
