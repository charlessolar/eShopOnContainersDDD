import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import FormHelperText from '@material-ui/core/FormHelperText';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import { WithStyles, withStyles } from '@material-ui/core/styles';
import { observer } from 'mobx-react';
import * as React from 'react';
import Dropzone, { ImageFile } from 'react-dropzone';
import * as ReactCrop from 'react-image-crop';
// tslint:disable-next-line:no-submodule-imports
import 'react-image-crop/dist/ReactCrop.css';
import { Data, urlToBase64 } from '../../utils/image';

interface ImageProps {
  id: string;
  required?: boolean;
  label: string;
  error?: any;
  value?: Data;
  fieldProps?: any;
  imageRatio?: number;
  disabled?: boolean;

  onChange?: (newVal: Data) => void;
}
interface ImageState {
  file?: ImageFile;
  crop?: ReactCrop.Crop;
  value?: Data;
}

const styles = theme => ({
  formControl: {
    marginLeft: theme.spacing.unit,
    marginRight: theme.spacing.unit,
    width: 400,
  },
});

@observer
class ImageControl extends React.Component<ImageProps & WithStyles<'formControl'>, ImageState> {
  constructor(props) {
    super(props);

    const { value } = props;

    this.state = {
      value
    };
  }

  private onCropped = (crop: ReactCrop.Crop, pixelCrop: ReactCrop.PixelCrop) => {
    this.setState({crop});
  }
  private onCropComplete = async (crop: ReactCrop.Crop, pixelCrop: ReactCrop.PixelCrop) => {
    const { file } = this.state;

    const result = await urlToBase64(file.preview, pixelCrop);
    result.filename = file.name;
    this.props.onChange(result);
  }

  private onImageLoaded = (image: HTMLImageElement) => {
    const { imageRatio } = this.props;

    this.setState({
      crop: ReactCrop.makeAspectCrop({
        x: 10,
        y: 10,
        width: 100,
        height: 100,
        aspect: imageRatio,
      }, image.naturalWidth / image.naturalHeight)
    });
  }

  private clearImage() {
    this.setState({ file: undefined, value: undefined });
    this.props.onChange(undefined);
  }

  private renderImage = () => {
    const { file, crop, value } = this.state;

    if (value) {
      return (
        <>
        <img src={value.url} alt='preview' style={{maxWidth: 300}}/>
        <Button onClick={() => this.clearImage()} color='primary' variant='raised'>Clear</Button>
        </>
      );
    }

    if (file === undefined) {
      return (
        <Dropzone multiple={false} accept='image/*' onDrop={(accepted) => this.handleDrop(accepted[0])}>
          <p>Drop an image or click to select a file to upload.</p>
        </Dropzone>
      );
    }

    return (
      <>
      <ReactCrop crop={crop} src={file.preview} onImageLoaded={this.onImageLoaded} onComplete={this.onCropComplete} onChange={this.onCropped} />
      <Button onClick={() => this.clearImage()} color='primary' variant='raised'>Clear</Button>
      </>
    );
  }

  private handleDrop(file: ImageFile) {
    this.setState({
      file
    });
  }

  public render() {
    const { id, label, required, error, value, disabled, classes, fieldProps } = this.props;

    return (
      <FormControl required={required} className={classes.formControl} disabled={disabled} error={error && error[id] ? true : false} aria-describedby={id + '-text'}>
        <Grid container spacing={24}>
          <Grid item xs={4}>
            <Typography variant='title' color={error && error[id] ? 'error' : 'primary'}>{label}</Typography>
            {error && error[id] ? error[id].map((e, key) => (<FormHelperText key={key} id={id + '-' + key + '-text'}>{e}</FormHelperText>)) : undefined}
          </Grid>
          <Grid item xs={8}>
            {this.renderImage()}
          </Grid>
        </Grid>
      </FormControl>
    );
  }
}
export default withStyles(styles)<ImageProps>(ImageControl);
