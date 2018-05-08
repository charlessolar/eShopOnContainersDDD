import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';

import { inject, models } from '../../../utils';
import { Using, Field, Submit } from '../../../components/models';

import { StoreType } from '../../../stores';
import { TypesStoreModel, TypesStoreType } from '../stores/types';

interface TypesProps {
  store: TypesStoreType;
}

@observer
export default class TypeView extends React.Component<TypesProps, {}> {

  public render() {
    const { store } = this.props;

    return (
      <div>
        <h3>Hello world</h3>
        <ul>
        {Array.from(store.list.entries.values(), (type, key) => (
          <li key={key}>{type.type} <Button onClick={() => store.list.remove(type.id)}>Remove</Button></li>
        ))}
        </ul>

        <h4>New Type</h4>

        <Using model={store.form}>
          <Field field='type' />
          <Submit/>
        </Using>
      </div>
    );

    // <Input property='type' onChange={}
    // <Text id='type' required label='Type:' error={this._type.payload.validation} onChange={(val) => this.handleChange(val)} />
  }
}
