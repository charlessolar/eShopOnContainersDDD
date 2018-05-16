import * as React from 'react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Paper from 'material-ui/Paper';
import Grid from 'material-ui/Grid';
import Button from 'material-ui/Button';
import Dialog from 'material-ui/Dialog';
import Divider from 'material-ui/Divider';
import Typography from 'material-ui/Typography';
import AppBar from 'material-ui/AppBar';
import Toolbar from 'material-ui/Toolbar';
import Tooltip from 'material-ui/Tooltip';
import Slide from 'material-ui/transitions/Slide';
import IconButton from 'material-ui/IconButton';
import CloseIcon from '@material-ui/icons/Close';
import AddIcon from '@material-ui/icons/Add';
import EditIcon from '@material-ui/icons/Edit';

import { inject } from '../../../utils';
import { Using, Field, Submit, Action } from '../../../components/models';
import Confirm from '../../../components/dialogs/confirm';

import { ProductType } from '../models/products';
import { ProductFormType, ProductFormModel } from '../stores/product';
import { CatalogStoreType } from '../stores/catalog';

const styles = (theme: Theme) => ({
  root: {
    marginTop: 20,
  },
  container: {
    marginTop: 20,
    width: '100%'
  },
  appBar: {
    position: 'relative',
  },
  divider: {
    backgroundColor: theme.palette.error[500],
    marginBottom: 50,
    marginTop: 50,
    marginRight: 50
  },
  flex: {
    flex: 1,
  },
  button: {
    margin: theme.spacing.unit,
  },
  dangerButton: {
    backgroundColor: theme.palette.error[500],
    color: theme.palette.primary.contrastText
  }
});

interface FormProps {
  handleClose: () => void;
  handleSuccess: (product: Partial<ProductType>) => void;

  list: CatalogStoreType;
  product?: ProductType;
  store?: ProductFormType;
}

@inject(ProductFormModel, 'store', 'product', (s: ProductType) => {
  return {
    id: s.id,
    name: s.name,
    description: s.description,
    price: s.price,
    catalogBrand: { id: s.catalogBrandId, brand: s.catalogBrand },
    catalogType: { id: s.catalogTypeId, type: s.catalogType },
    picture: s.pictureContents ? {
      data: s.pictureContents,
      contentType: s.pictureContentType
    } : undefined
  };
})
class FormView extends React.Component<FormProps & WithStyles<'root' | 'container' | 'appBar' | 'flex' | 'button' | 'divider' | 'dangerButton'>, {}> {
  private handleSuccess = () => {
    const { store, handleClose, handleSuccess } = this.props;

    handleSuccess(store.partial);
    handleClose();
  }
  private handleDestroy = () => {
    const { list, product, handleClose } = this.props;
    list.remove(product.id);
    handleClose();
  }

  public render() {
    const { classes, store, product, handleClose } = this.props;

    return (
      <Using model={store}>
      <AppBar className={classes.appBar}>
        <Toolbar>
          <IconButton color='inherit' onClick={handleClose} aria-label='Close'>
            <CloseIcon />
          </IconButton>
          <Typography variant='title' color='inherit' className={classes.flex}>
            {product ? 'Edit' : 'Add'} Product
          </Typography>
          <Submit buttonProps={{ color: 'inherit' }} onSuccess={this.handleSuccess} />
        </Toolbar>
      </AppBar>
      <div className={classes.container}>
        <Grid container justify='center'>
          <Grid item xs={8}>
            <Paper elevation={4}>
              <Grid container spacing={40}>
                <Grid item md={6} xs={12}>
                  <Field field='picture' />
                </Grid>
                <Grid item md={6} xs={12}>
                  <Grid container spacing={24}>
                    <Grid item xs={12}>
                      <Field field='name' />
                    </Grid>
                    <Grid item xs={12}>
                      <Field field='description' />
                    </Grid>
                    <Grid item xs={12}>
                      <Field field='price' />
                    </Grid>
                    <Grid item xs={12}>
                      <Field field='catalogType' />
                    </Grid>
                    <Grid item xs={12}>
                      <Field field='catalogBrand' />
                    </Grid>
                    <Grid item xs={12}>
                      <Divider className={classes.divider}/>
                      { product &&
                        <>
                          <Confirm title='Confirm Destroy' description='Destroying a product is non-reversable!'>
                            <Action action='destroy' onSuccess={this.handleDestroy} text='Destroy' buttonProps={{ className: classes.dangerButton }}/>
                          </Confirm>
                        </>
                      }
                    </Grid>
                  </Grid>
                </Grid>
              </Grid>
            </Paper>
          </Grid>
        </Grid>
      </div>
    </Using>
    );
  }
}

export default withStyles(styles as any)<FormProps>(FormView);
