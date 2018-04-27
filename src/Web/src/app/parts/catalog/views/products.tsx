import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';

import { inject, models, FormType } from '../../../utils';
import { Form, Field } from '../../../components/forms';

import { StoreType } from '../../../stores';
import { Product, Products, ProductFormType, ProductsType } from '../models/products';

interface ProductsProps {
  store: ProductsType;
}

@observer
export default class ProductView extends React.Component<ProductsProps, {}> {
  private _product: FormType<ProductFormType>;

  constructor(props) {
    super(props);

    this._product = props.store.productForm();
  }

  public handleChange(newVal: string) {
    const { store } = this.props;

  }
  public handleSubmit() {
    const { store } = this.props;

    if (!this._product.valid) {
      return;
    }

    const payload = this._product.payload;
    store.List.add({
      id: payload.id,
      name: payload.name,
      description: payload.description,
      price: payload.price,
      catalogBrand: '',
      catalogBrandId: payload.catalogBrandId,
      catalogType: '',
      catalogTypeId: payload.catalogTypeId
    });
  }

  public render() {
    const { store } = this.props;

    return (
      <div>
        <h3>Hello world</h3>
        <ul>
        {Array.from(store.List.entries.values(), (product, key) => (
          <li key={key}>{product.name} <Button onClick={() => store.List.remove(product.id)}>Remove</Button></li>
        ))}
        </ul>

        <h4>New Product</h4>

        <Form form={this._product}>
          <Field field='name' />
          <Field field='description' />
          <Field field='price' />
          <Field field='catalogTypeId' />
          <Field field='catalogBrandId' />
        </Form>
      </div>
    );

    // <Input property='brand' onChange={}
    // <Text id='brand' required label='Brand:' error={this._brand.payload.validation} onChange={(val) => this.handleChange(val)} />
  }
}
