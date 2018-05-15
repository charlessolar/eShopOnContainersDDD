import * as React from 'react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';
import Dialog from 'material-ui/Dialog';
import Slide from 'material-ui/transitions/Slide';

import { ProductType } from '../../../models/catalog/products';
import { ProductFormType } from '../stores/product';
import { CatalogStoreType } from '../stores/catalog';

import FormView from '../components/productForm';

interface ProductFormProps {
  list: CatalogStoreType;

  store?: ProductFormType;
}

function Transition(props) {
  return <Slide direction='up' {...props} />;
}

const styles = theme => ({
  root: {
    marginTop: 20,
  }
});

class ProductFormAddView extends React.Component<ProductFormProps & WithStyles<'root'>, { open: boolean }> {
  constructor(props) {
    super(props);
    this.state = {
      open: false
    };
  }

  private handleClickOpen = () => {
    this.setState({ open: true });
  }

  private handleClose = () => {
    this.setState({ open: false });
  }

  private handleSuccess = (product: ProductType) => {
    const { list } = this.props;
    list.add(product);
  }

  public render() {
    const { classes, store, list } = this.props;

    return (
      <div className={classes.root}>
        <Button variant='raised' color='primary' size='large' onClick={this.handleClickOpen}>Create Product</Button>

        <Dialog
          fullScreen
          open={this.state.open}
          onClose={this.handleClose}
          TransitionComponent={Transition}
        >
          <FormView {...this.props} list={list} handleSuccess={this.handleSuccess} handleClose={this.handleClose}/>
        </Dialog>
      </div>
    );
  }
}
export default withStyles(styles as any)<ProductFormProps>(ProductFormAddView);
