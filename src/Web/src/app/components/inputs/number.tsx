import FormControl from '@material-ui/core/FormControl';
import FormHelperText from '@material-ui/core/FormHelperText';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import { WithStyles, withStyles } from '@material-ui/core/styles';
import * as numeral from 'numeral';
import * as React from 'react';

interface NumberProps {
  id: string;
  required?: boolean;
  label: string;
  error?: any;
  type?: string;
  value?: any;
  autoComplete?: string;
  format?: string;
  fieldProps?: any;
  normalize?: number;
  disabled?: boolean;

  onChange?: (newVal: number) => void;
  onKeyDown?: (key: number, value: string) => void;
}

class NumberControl extends React.Component<NumberProps & WithStyles<'formControl'>, { value: string }> {
  constructor(props) {
    super(props);

    const { value, normalize } = props;
    if (!value) {
      this.state = { value: '' };
      return;
    }

    let num = numeral(value).value();
    if (normalize) {
      num /= Math.pow(10, normalize);
    }

    this.state = { value: num + '' };
  }

  public componentWillReceiveProps(nextProps: NumberProps) {
    if (this.props.value !== nextProps.value) {
      const { value, normalize } = nextProps;

      if (!value) {
        this.setState({ value: '' });
        return;
      }

      let num = numeral(value).value();
      if (normalize) {
        num /= Math.pow(10, normalize);
      }
      this.setState({ value: num + '' });
    }
  }

  private onBlur = () => {
    const { normalize, value } = this.props;

    let num = numeral(value).value();

    if (normalize) {
      num /= Math.pow(10, normalize);
    }

    this.setState({value: num + ''});
  }

  private handleChange = (e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
    const { normalize, onChange } = this.props;

    this.setState({ value: e.target.value });

    if (onChange) {
      let num = numeral(e.target.value).value();

      if (normalize) {
        num *= Math.pow(10, normalize);
      }

      this.props.onChange(Math.round(num));
    }
  }
  private handleKeydown = (e: React.KeyboardEvent<HTMLTextAreaElement | HTMLInputElement>) => {
    if (this.props.onKeyDown) {
      this.props.onKeyDown(e.which, (e.target as any).value.trim());
    }
  }

  public render() {
    const { id, label, required, error, type, autoComplete, disabled, classes, fieldProps } = this.props;

    return (
      <FormControl required={required} className={classes.formControl} disabled={disabled} fullWidth error={error && error[id] ? true : false} aria-describedby={id + '-text'}>
        <InputLabel htmlFor={id}>{label}</InputLabel>
        <Input id={id} onBlur={this.onBlur} onChange={this.handleChange} type={type || 'text'} autoComplete={autoComplete} value={this.state.value} onKeyDown={this.handleKeydown} {...fieldProps} />
        {error && error[id] ? error[id].map((e, key) => (<FormHelperText key={key} id={id + '-' + key + '-text'}>{e}</FormHelperText>)) : undefined}
      </FormControl>
    );
  }
}

const styles = theme => ({
  formControl: {
    marginLeft: theme.spacing.unit,
    marginRight: theme.spacing.unit,
    maxWidth: 400,
  },
});

export default withStyles(styles)<NumberProps>(NumberControl);
