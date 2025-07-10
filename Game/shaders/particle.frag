#version 330 core

in vec2 TexCoord;
out vec4 FragColor;

uniform sampler2D particleTexture;
uniform float alpha;

void main(){
    vec4 texColor = texture(particleTexture, TexCoord);
    FragColor = vec4(texColor.rgb, texColor.a * alpha);
}
