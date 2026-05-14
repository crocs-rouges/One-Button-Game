shader_type canvas_item;

uniform vec4 outline_color : source_color = vec4(1.0, 0.84, 0.0, 1.0); // Gold
uniform float outline_width = 3.0;

void fragment() 
{
    vec4 lCol = texture(TEXTURE, UV);
    vec2 lSize = TEXTURE_PIXEL_SIZE * outline_width;
    float lOutline = texture(TEXTURE, UV + vec2(-lSize.x, 0.0)).a;
    lOutline += texture(TEXTURE, UV + vec2(0.0, lSize.y)).a;
    lOutline += texture(TEXTURE, UV + vec2(lSize.x, 0.0)).a;
    lOutline += texture(TEXTURE, UV + vec2(0.0, -lSize.y)).a;
    lOutline += texture(TEXTURE, UV + vec2(-lSize.x, lSize.y)).a;
    lOutline += texture(TEXTURE, UV + vec2(lSize.x, lSize.y)).a;
    lOutline += texture(TEXTURE, UV + vec2(-lSize.x, -lSize.y)).a;
    lOutline += texture(TEXTURE, UV + vec2(lSize.x, -lSize.y)).a;
    lOutline = min(lOutline, 1.0);
    if (lCol.a < 0.0001 && lOutline > 0.0) COLOR = outline_color;
    else COLOR = lCol;        
}
