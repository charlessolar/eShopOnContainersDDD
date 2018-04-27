import * as React from 'react';
import { observable, action, computed } from 'mobx';
import { observer, inject } from 'mobx-react';
import { IModelType } from 'mobx-state-tree';
import * as keycode from 'keycode';
import Downshift from 'downshift';

import { withStyles, WithStyles } from 'material-ui/styles';
import { MenuItem } from 'material-ui/Menu';
import TextField from 'material-ui/TextField';
import Paper from 'material-ui/Paper';
import Chip from 'material-ui/Chip';

import { SelectType, models } from '../../utils';

const renderInput = (onFocus, onChange, inputProps) => {
  const { InputProps, classes, ref, ...other } = inputProps;

  return (
    <TextField
      onFocus={onFocus}
      onChange={onChange}
      InputProps={{
        inputRef: ref,
        classes: {
          root: classes.inputRoot,
        },
        ...InputProps,
      }}
      {...other}
    />
  );
};
const renderSuggestion = ({ suggestion, index, itemProps, highlightedIndex, selectedItem }) => {
  const isHighlighted = highlightedIndex === index;
  const isSelected = selectedItem && selectedItem === suggestion.label;

  return (
    <MenuItem
      {...itemProps}
      key={suggestion.label}
      selected={isHighlighted}
      component='div'
      style={{
        fontWeight: isSelected ? 'bold' : 'normal',
      }}
    >
      {suggestion.label}
    </MenuItem>
  );
};

interface DownshiftProps {

}
interface DownshiftState {
  inputValue: string;
  selectedItem: string[];
}

const styles = theme => ({
  root: {
    flexGrow: 1,
    height: 250,
  },
  container: {
    flexGrow: 1,
    position: 'relative',
  },
  paper: {
    position: 'absolute',
    zIndex: 1,
    marginTop: theme.spacing.unit,
    left: 0,
    right: 0,
  }
});

interface SelectProps {
  id: string;
  required?: boolean;
  label: string;
  error?: any;
  type?: string;
  value?: any;
  onChange?: (newVal: string) => void;

  projectionStore?: IModelType<{}, { list(): Promise<{}>, updateTerm(val: string): Promise<{}> }>;
  projection?: (store: any, term: string) => Promise<{ id: string, label: string}[]>;
}

class IntegrationDownshift extends React.Component<SelectProps & WithStyles<'root' | 'container' | 'paper'>, {}> {
  private _store: SelectType;
  private _projectionStore: any;

  constructor(props: any) {
    super(props);

    this._projectionStore = props.store.create(props.projectionStore);
    this._store = models.select(props.store, this._projectionStore, props.projection);
  }

  public onFocus() {
    this._store.list();
  }
  public onChange(newVal: string) {
    this._store.updateTerm(newVal);
  }
  public selectionChanged(selection: { id: string, label: string }) {
    const { onChange } = this.props;
    onChange(selection.id);
  }

  public render() {
    const { id, label, required, error, type, value, classes } = this.props;

    return (
      <div className={classes.root}>
        <Downshift onChange={(val) => this.selectionChanged(val)} itemToString={item => item.label}>
          {({ getInputProps, getItemProps, isOpen, inputValue, selectedItem, highlightedIndex }) => (
            <div className={classes.container}>
              {renderInput(
                () => this.onFocus(),
                (newVal: string) => this.onChange(newVal),
                {
                  fullWidth: true,
                  classes,
                  InputProps: getInputProps({
                    placeholder: label,
                    id,
                  }),
              })}
              {isOpen ? (
                <Paper className={classes.paper} square>
                  {this._store.records.map((suggestion, index) =>
                    renderSuggestion({
                      suggestion,
                      index,
                      itemProps: getItemProps({ item: suggestion }),
                      highlightedIndex,
                      selectedItem,
                    }),
                  )}
                </Paper>
              ) : null}
            </div>
          )}
        </Downshift>
      </div>
    );
  }
}

export default inject('store')(withStyles(styles as any)(IntegrationDownshift));
