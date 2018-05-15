import * as React from 'react';
import { observer } from 'mobx-react';

import { UsingContext, UsingType } from './using';
import { Text, TextArea, Number, Selecter, Dropdown, Checkbox, Image, DateRange } from '../inputs';

import { Data } from '../../utils/image';

interface FieldProps {
  field: string;
  fieldProps?: any;
}

export class Field extends React.Component<FieldProps, {}> {

  public render() {
    const { field, fieldProps } = this.props;

    return (
      <UsingContext.Consumer>
        {value => (
          <FieldConsumer using={value} field={field} fieldProps={fieldProps} />
        )}
      </UsingContext.Consumer>
    );
  }
}

const FieldConsumer = observer((props: FieldProps & { using: UsingType<any> }) => {

  const { using, field, fieldProps } = props;
  const definition = using.form[field];

  if (!definition) {
    throw new Error('No definition for field ' + field + ' was found!');
  }

  const handleChange = (val: string | number | Data | { from: string, to: string} | any) => {
    using.changeValue(field, val);
  };
  const handleCheckboxChange = (val: string, checked: boolean) => {
    if (checked) {
      using.pushValue(field, val);
    } else {
      using.removeValue(field, val);
    }
  };

  switch (definition.input) {
    case 'text':
      return React.createElement(Text, {
        id: field,
        value: using.payload[field],
        required: definition.required,
        error: using.validation,
        label: definition.label,
        autoComplete: definition.autoComplete,
        onChange: handleChange,
        fieldProps
      });
    case 'password':
      return React.createElement(Text, {
        id: field,
        type: 'password',
        value: using.payload[field],
        required: definition.required,
        error: using.validation,
        label: definition.label,
        autoComplete: definition.autoComplete,
        onChange: handleChange,
        fieldProps
      });
    case 'selecter':
      return React.createElement(Selecter, {
        id: field,
        value: using.payload[field],
        required: definition.required,
        error: using.validation,
        label: definition.label,
        onChange: handleChange,
        selectStore: definition.selectStore,
        addComponent: definition.addComponent
      });
    case 'dropdown':
      return React.createElement(Dropdown, {
        id: field,
        value: using.payload[field],
        required: definition.required,
        error: using.validation,
        label: definition.label,
        options: definition.options,
        onChange: handleChange,
        allowEmpty: definition.allowEmpty
      });
    case 'textarea':
      return React.createElement(TextArea, {
        id: field,
        value: using.payload[field],
        required: definition.required,
        error: using.validation,
        label: definition.label,
        autoComplete: definition.autoComplete,
        onChange: handleChange,
        fieldProps
      });
    case 'number':
      return React.createElement(Number, {
        id: field,
        value: using.payload[field],
        required: definition.required,
        error: using.validation,
        label: definition.label,
        autoComplete: definition.autoComplete,
        onChange: handleChange,
        normalize: definition.normalize,
        fieldProps
      });
    case 'checkbox':
      return React.createElement(Checkbox, {
        id: field,
        value: using.payload[field],
        required: definition.required,
        error: using.validation,
        label: definition.label,
        options: definition.options,
        onChange: handleCheckboxChange,
      });
    case 'image':
      return React.createElement(Image, {
        id: field,
        value: using.payload[field],
        required: definition.required,
        error: using.validation,
        label: definition.label,
        onChange: handleChange,
        imageRatio: definition.imageRatio,
        fieldProps
      });
    case 'daterange':
      return React.createElement(DateRange, {
        id: field,
        value: using.payload[field],
        required: definition.required,
        error: using.validation,
        label: definition.label,
        onChange: handleChange,
      });
    default:
      throw new Error('unknown input type: ' + definition.input);
  }
});
