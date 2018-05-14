import * as React from 'react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Paper from 'material-ui/Paper';
import Grid from 'material-ui/Grid';
import Button from 'material-ui/Button';
import Dialog from 'material-ui/Dialog';
import Typography from 'material-ui/Typography';
import AppBar from 'material-ui/AppBar';
import Toolbar from 'material-ui/Toolbar';
import Slide from 'material-ui/transitions/Slide';
import IconButton from 'material-ui/IconButton';
import CloseIcon from '@material-ui/icons/Close';

import { inject } from '../../../utils';
import { Using, Field, Submit } from '../../../components/models';

import { ProductFormType, ProductFormModel } from '../stores/product';
import { CatalogStoreType } from '../stores/catalog';

interface ProductFormProps {
  list: CatalogStoreType;

  store?: ProductFormType;
}

const styles = theme => ({
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
  flex: {
    flex: 1,
  },
});

function Transition(props) {
  return <Slide direction='up' {...props} />;
}

@inject(ProductFormModel)
class ProductFormView extends React.Component<ProductFormProps & WithStyles<'root' | 'container' | 'appBar' | 'flex'>, { open: boolean }> {
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

  private handleSuccess = () => {
    const { list, store } = this.props;

    // list.add(store);
    list.add({
      id: store.id,
      name: store.name,
      description: store.description,
      price: store.price,
      catalogBrand: store.catalogBrand.brand,
      catalogBrandId: store.catalogBrand.id,
      catalogType: store.catalogType.type,
      catalogTypeId: store.catalogType.id,
      availableStock: 0,
      restockThreshold: 0,
      maxStockThreshold: 0,
      onReorder: false,
    });

    this.handleClose();
  }
  public render() {
    const { classes, store } = this.props;

    return (
      <div className={classes.root}>
        <Button variant='raised' color='primary' size='large' onClick={this.handleClickOpen}>Create Product</Button>

        <Dialog
          fullScreen
          open={this.state.open}
          onClose={this.handleClose}
          TransitionComponent={Transition}
        >
          <Using model={store}>
            <AppBar className={classes.appBar}>
              <Toolbar>
                <IconButton color='inherit' onClick={this.handleClose} aria-label='Close'>
                  <CloseIcon />
                </IconButton>
                <Typography variant='title' color='inherit' className={classes.flex}>
                  Add Product
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
                        </Grid>
                      </Grid>
                    </Grid>
                  </Paper>
                </Grid>
              </Grid>
            </div>
          </Using>
        </Dialog>
      </div>
    );
  }
}

export default withStyles(styles as any)<ProductFormProps>(ProductFormView);
