import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';

import { inject, models } from '../../../utils';
import { Using, Field, Submit } from '../../../components/models';

import { StoreType } from '../../../stores';
import { BrandsStoreModel, BrandsStoreType } from '../stores/brands';

interface BrandsProps {
  store: BrandsStoreType;
}

@observer
export default class BrandView extends React.Component<BrandsProps, {}> {

  public render() {
    const { store } = this.props;

    return (
      <div>
        <h3>Hello world</h3>
        <ul>
        {Array.from(store.list.entries.values(), (brand, key) => (
          <li key={key}>{brand.brand} <Button onClick={() => store.list.remove(brand.id)}>Remove</Button></li>
        ))}
        </ul>

        <h4>New Brand</h4>

        <Using model={store.form}>
          <Field field='brand' />
          <Submit/>
        </Using>
      </div>
    );

    // <Input property='brand' onChange={}
    // <Text id='brand' required label='Brand:' error={this._brand.payload.validation} onChange={(val) => this.handleChange(val)} />
  }
}
