import * as React from 'react';

import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';

import { inject, models, FormType } from '../../utils';
import { Text, Select, TextArea, Number } from '../inputs';

interface FormProps<T> {
  form: FormType<T>;
}

@observer
export class Form<T> extends React.Component<FormProps<T>, {}> {

  public handleSubmit() {
    const { form } = this.props;

    form.submit();
  }

  public render() {
    const { children, form } = this.props;

    const childrenWithProps = React.Children.map(children, child => React.cloneElement(child as any, { form }));

    return (
      <form>
        {childrenWithProps}
        <Button disabled={!form.valid || form.loading} onClick={() => this.handleSubmit()}>Save</Button>
      </form>
    );
  }
}

interface FieldProps<T> {
  form?: FormType<T>;
  field: string;
}

@observer
export class Field<T> extends React.Component<FieldProps<T>, {}> {

  public handleChange(val: string) {
    const { field, form } = this.props;

    form.changeValue(field, val);
  }

  public render() {
    const { field, form } = this.props;

    const definition = form.form[field];

    switch (definition.input) {
      case 'text':
        return React.createElement(Text, {
          id: field,
          required: definition.required,
          error: form.validation,
          label: definition.label,
          onChange: (val) => this.handleChange(val)
        });
      case 'select':
        return React.createElement(Select, {
          id: field,
          required: definition.required,
          error: form.validation,
          label: definition.label,
          onChange: (val) => this.handleChange(val),
          projectionStore: definition.projectionStore,
          projection: definition.projection
        });
        case 'textarea':
          return React.createElement(TextArea, {
            id: field,
            required: definition.required,
            error: form.validation,
            label: definition.label,
            onChange: (val) => this.handleChange(val)
          });
          case 'number':
            return React.createElement(Number, {
              id: field,
              required: definition.required,
              error: form.validation,
              label: definition.label,
              onChange: (val) => this.handleChange(val)
            });
      default:
        throw new Error('unknown input type: ' + definition.input);
    }
  }
}
