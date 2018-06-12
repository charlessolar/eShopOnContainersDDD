import Dialog from '@material-ui/core/Dialog';
import IconButton from '@material-ui/core/IconButton';
import Slide from '@material-ui/core/Slide';
import Tooltip from '@material-ui/core/Tooltip';
import { WithStyles, withStyles } from '@material-ui/core/styles';
import EditIcon from '@material-ui/icons/Edit';
import { IStateTreeNode, applyPatch } from 'mobx-state-tree';
import * as React from 'react';
import { ProductType } from '../../models/products';
import { CatalogStoreType } from '../../stores/catalog';
import { ProductFormType } from '../../stores/product';
import FormView from './ProductForm';

interface ProductFormProps {
  product: ProductType;
  list: CatalogStoreType;

  store?: ProductFormType;
}

function Transition(props) {
  return <Slide direction='up' {...props} />;
}

const styles = theme => ({
  root: {
  },
  button: {
    color: theme.palette.primary.light
  }
});

class ProductFormEditView extends React.Component<ProductFormProps & WithStyles<'root' | 'button'>, { open: boolean }> {
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

  private handleSuccess = (patch: Partial<ProductType>) => {
    const { product } = this.props;

    if (patch.name) {
      applyPatch(product as IStateTreeNode, { op: 'replace', path: '/name', value: patch.name });
    }
    if (patch.price) {
      applyPatch(product as IStateTreeNode, { op: 'replace', path: '/price', value: patch.price });
    }
    if (patch.description) {
      applyPatch(product as IStateTreeNode, { op: 'replace', path: '/description', value: patch.description });
    }
    if (patch.pictureContents) {
      applyPatch(product as IStateTreeNode, { op: 'replace', path: '/pictureContents', value: patch.pictureContents });
    }
  }

  public render() {
    const { classes, store, product, list } = this.props;

    return (
      <div className={classes.root}>
      <Tooltip title='Edit Product'>
        <IconButton className={classes.button} onClick={this.handleClickOpen} aria-label='Edit Product'><EditIcon /></IconButton>
      </Tooltip>

        <Dialog
          fullScreen
          open={this.state.open}
          onClose={this.handleClose}
          TransitionComponent={Transition}
        >
          <FormView {...this.props} product={product} list={list} handleSuccess={this.handleSuccess} handleClose={this.handleClose}/>
        </Dialog>
      </div>
    );
  }
}

export default withStyles(styles as any)<ProductFormProps>(ProductFormEditView);
