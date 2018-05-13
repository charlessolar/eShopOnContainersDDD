import * as React from 'react';
import { observer } from 'mobx-react';
import { hot } from 'react-hot-loader';
import glamorous from 'glamorous';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import { ListItem, ListItemText } from 'material-ui/List';
import Button from 'material-ui/Button';
import Grid from 'material-ui/Grid';
import Paper from 'material-ui/Paper';
import Typography from 'material-ui/Typography';
import Avatar from 'material-ui/Avatar';
import Table, { TableBody, TableCell, TableHead, TableRow } from 'material-ui/Table';

import { Using, Formatted } from '../../../components/models';
import { CatalogStoreType, CatalogStoreModel } from '../stores/catalog';

interface CatalogProps {
  store?: CatalogStoreType;
}

const styles = theme => ({
  root: {
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto',
  },
  table: {
  },
  avatar: {
    margin: 10,
    width: 60,
    height: 60
  },
  row: {
    '&:nth-of-type(odd)': {
      backgroundColor: theme.palette.background.default,
    },
  },
});

@observer
class CatalogView extends React.Component<CatalogProps & WithStyles<'root' | 'table' | 'avatar' | 'row'>, {}> {

  public render() {
    const { store, classes } = this.props;

    const products = Array.from(store.products.values());
    return (
      <Grid container justify='center'>
        <Grid item xs={8}>
          <Paper className={classes.root}>
            <Table className={classes.table}>
              <TableHead>
                <TableRow>
                  <TableCell>Product Name</TableCell>
                  <TableCell>Catagory Type</TableCell>
                  <TableCell>Catagory Brand</TableCell>
                  <TableCell>Price ($)</TableCell>
                  <TableCell>Available Stock</TableCell>
                  <TableCell>Thresholds</TableCell>

                </TableRow>
              </TableHead>
              <TableBody>
                {products.map(p => (
                  <TableRow hover key={p.id} className={classes.row}>
                    <Using model={p}>
                      <TableCell component='th' scope='row'>
                        <ListItem>
                          <Avatar src={p.productPicture} className={classes.avatar} />
                          <ListItemText primary={p.name} secondary={p.description} />
                        </ListItem>
                      </TableCell>
                      <TableCell>{p.catalogType}</TableCell>
                      <TableCell>{p.catalogBrand}</TableCell>
                      <TableCell numeric><Formatted field='price' /></TableCell>
                      <TableCell numeric><Formatted field='availableStock' /></TableCell>
                      <TableCell>
                        <Typography variant='body2'>Restock: <Formatted field='restockThreshold' /></Typography>
                        <Typography variant='body2'>Max Stock: <Formatted field='maxStockThreshold' /></Typography>
                      </TableCell>
                    </Using>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </Paper>
        </Grid>
      </Grid>
    );
  }
}

export default withStyles(styles as any)(CatalogView);
