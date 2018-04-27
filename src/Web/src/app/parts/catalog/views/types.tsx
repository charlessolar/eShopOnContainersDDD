import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';

import { inject, models, FormType } from '../../../utils';
import { Form, Field } from '../../../components/forms';

import { StoreType } from '../../../stores';
import { Type, Types, TypeFormType, TypesType } from '../models/types';

interface TypesProps {
  store: TypesType;
}

@observer
export default class TypeView extends React.Component<TypesProps, {}> {
  private _type: FormType<TypeFormType>;

  constructor(props) {
    super(props);

    this._type = props.store.typeForm();
  }

  public handleChange(newVal: string) {
    const { store } = this.props;

  }
  public handleSubmit() {
    const { store } = this.props;

    if (!this._type.valid) {
      return;
    }

    store.List.add(this._type.payload);
  }

  public render() {
    const { store } = this.props;

    return (
      <div>
        <h3>Hello world</h3>
        <ul>
        {Array.from(store.List.entries.values(), (type, key) => (
          <li key={key}>{type.type} <Button onClick={() => store.List.remove(type.id)}>Remove</Button></li>
        ))}
        </ul>

        <h4>New Type</h4>

        <Form form={this._type}>
          <Field field='type' />
        </Form>
      </div>
    );

    // <Input property='type' onChange={}
    // <Text id='type' required label='Type:' error={this._type.payload.validation} onChange={(val) => this.handleChange(val)} />
  }
}
