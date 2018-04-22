
interface Avatar {
  file_data: string;
  file_name: string;
  content_type: string;
}
export interface ProfileRequest {
  password?: string;
  avatar?: Avatar;
  first_name?: string;
  last_name?: string;
  phone?: string;
  birth_date?: string;
}
export interface ProfileResponse {
  email: string;
  avatar?: Avatar;
  first_name: string;
  last_name: string;
  phone: string;
  birth_date: string;
}
