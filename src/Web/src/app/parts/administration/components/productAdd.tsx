import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import Slide from '@material-ui/core/Slide';
import { WithStyles, withStyles } from '@material-ui/core/styles';
import * as React from 'react';
import { ProductType } from '../../../models/catalog/products';
import FormView from '../components/productForm';
import { CatalogStoreType } from '../stores/catalog';
import { ProductFormType } from '../stores/product';

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
