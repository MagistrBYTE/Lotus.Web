import { ISelectOption, TKey } from 'lotus-core';
import { ReactNode, useState } from 'react';
import Select, { ActionMeta, OptionProps, Props, SingleValue } from 'react-select';
import { ILabelProps, Label } from 'ui/components/Display/Label';
import { HorizontalStack } from 'ui/components/Layout/HorizontalStack';
import { TColorType, TControlSize } from 'ui/types';
import './SelectOne.css';

export interface ISelectOneProps<TValueOption extends TKey = TKey> extends ILabelProps, Props<ISelectOption, false> 
{
  /**
   * Цвет
   */
  color?: TColorType;

  /**
   * Размер
   */
  size?: TControlSize;

  /**
   * Список опций
   */
  options: ISelectOption[];

  /**
   * Функция обратного вызова для установки выбранного значения
   * @param selectedValue Выбранное значение
   * @returns 
   */
  onSetSelectedValue?: (selectedValue: TValueOption) => void;

  /**
   * Изначально выбранное значение
   */
  initialSelectedValue?: TValueOption;

  /**
   * Дополнительный элемент справа
   */
  rightElement?: ReactNode;
}

export const SelectOne = <TValueOption extends TKey = TKey>({options, onSetSelectedValue, initialSelectedValue, 
  textInfo, textInfoKey, labelStyle, isTopLabel, rightElement, ...props}: ISelectOneProps<TValueOption>) => 
{
  const [selectedOption, setSelectedOption] = useState<ISelectOption|undefined>(options.find(x => x.value === initialSelectedValue));

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const handleSelect = (newValue: SingleValue<ISelectOption>, _actionMeta: ActionMeta<ISelectOption>) => 
  {
    const value = newValue?.value;
    setSelectedOption(newValue!);
    if(onSetSelectedValue)
    {
      onSetSelectedValue(value);
    }
  };

  const RenderItem = (props: OptionProps<ISelectOption>) => 
  {
    const {
      className,
      cx,
      isDisabled,
      isFocused,
      isSelected,
      innerRef,
      innerProps,
      data
    } = props;

    if(data.icon)
    {
      if(typeof data.icon === 'string')
      {
        return(
          <div
            ref={innerRef}
            className={cx(
              {
                option: true,
                'option--is-disabled': isDisabled,
                'option--is-focused': isFocused,
                'option--is-selected': isSelected
              },
              className
            )}
            {...innerProps}
          >
            <div style={{display: 'flex', flexDirection: 'row', justifyContent: 'flex-start', alignItems: 'center'}}>
              <img src={data.icon} width='32' height='32'/>
              <span style={{paddingLeft: 8}}>{data.text}</span>
            </div>
          </div>);
      }
      else
      {
        return (
          <div
            ref={innerRef}
            className={cx(
              {
                option: true,
                'option--is-disabled': isDisabled,
                'option--is-focused': isFocused,
                'option--is-selected': isSelected
              },
              className
            )}
            {...innerProps}
          >
            <div style={{display: 'flex', flexDirection: 'row', justifyContent: 'flex-start', alignItems: 'center'}}>
              <>
                {data.icon}
              </>
              <span style={{paddingLeft: 8}}>{data.text}</span>
            </div>
          </div>); 
      }
    }
    else
    {
      return(
        <div
          ref={innerRef}
          className={cx(
            {
              option: true,
              'option--is-disabled': isDisabled,
              'option--is-focused': isFocused,
              'option--is-selected': isSelected
            },
            className
          )}
          {...innerProps}
        >
          <span>{data.text}</span>
        </div>
      );
    }
  }  

  return (
    <Label
      label={props.label}
      labelStyle={labelStyle}
      isTopLabel={isTopLabel}
      fullWidth={props.fullWidth} 
      textInfo={textInfo} 
      textInfoKey={textInfoKey} >
      <HorizontalStack fullWidth> 
        <Select
          {...props}
          options={options}
          value={selectedOption}
          onChange={handleSelect}
          components={{Option: RenderItem}}
        />
        {rightElement}
      </HorizontalStack>
    </Label>
  );
}