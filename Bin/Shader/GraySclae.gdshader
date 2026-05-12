shader_type canvas_item;
void fragment() {
   vec4 tex = texture(TEXTURE, UV);
  float gray = 0.2126 * tex.r + 0.7152 * tex.g + 0.0722 * tex.b;
  COLOR = vec4(vec3(gray), tex.a);
}
