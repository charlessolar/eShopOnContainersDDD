import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';

import { inject, models } from '../../../utils';
import { Using, Field, Submit } from '../../../components/models';

import { StoreType } from '../../../stores';
import { ProductsStoreModel, ProductsStoreType } from '../stores/products';

interface ProductsProps {
  store: ProductsStoreType;
}

@observer
export default class ProductView extends React.Component<ProductsProps, {}> {

  public render() {
    const { store } = this.props;

    return (
      <div>
        <h3>Hello world</h3>
        <ul>
        {Array.from(store.list.entries.values(), (product, key) => (
          <li key={key}>{product.name} <Button onClick={() => store.list.remove(product.id)}>Remove</Button></li>
        ))}
        </ul>

        <h4>New Product</h4>

        <Using model={store.form}>
          <Field field='name' />
          <Field field='description' />
          <Field field='price' />
          <Field field='catalogType' />
          <Field field='catalogBrand' />
          <Submit/>
        </Using>
      </div>
    );

    // <Input property='brand' onChange={}
    // <Text id='brand' required label='Brand:' error={this._brand.payload.validation} onChange={(val) => this.handleChange(val)} />
  }
}
